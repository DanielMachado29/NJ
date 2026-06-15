# Arquitetura de Software — Resumo Prático (.NET)

Material de revisão para entrevista, baseado nos requisitos da vaga **Back-End Developer (.NET)** da Aliare.

---

## Mapa dos conceitos da vaga

| Conceito | Onde aparece | Prioridade |
|----------|--------------|------------|
| Arquitetura em camadas | Responsabilidades | Obrigatório |
| Clean Architecture | Responsabilidades + diferencial | Obrigatório / diferencial |
| DDD (nível prático) | Responsabilidades + diferencial | Obrigatório / diferencial |
| APIs REST escaláveis | Responsabilidades | Obrigatório |
| SOLID + padrões de projeto | Responsabilidades | Obrigatório |
| Mensageria / comunicação assíncrona | Responsabilidades + diferencial | Diferencial |
| Sistemas distribuídos / alta escala | Diferencial | Diferencial |
| Observabilidade | Responsabilidades + diferencial | Diferencial |
| Docker + Azure + CI/CD | Diferencial | Diferencial |

---

## Arquitetura em camadas (Layered Architecture)

Separa a aplicação em **camadas com responsabilidades distintas**. Cada camada só conhece a camada imediatamente abaixo (ou usa interfaces para inverter dependência).

```
┌─────────────────────────────────────┐
│  Apresentação (API / Controllers)   │  ← HTTP, DTOs, validação de entrada
├─────────────────────────────────────┤
│  Aplicação (Services / Use Cases)   │  ← orquestração, regras de fluxo
├─────────────────────────────────────┤
│  Domínio (Entities / Business Rules)│  ← regras de negócio puras
├─────────────────────────────────────┤
│  Infraestrutura (EF Core, APIs ext.)│  ← banco, filas, e-mail, cache
└─────────────────────────────────────┘
```

### Estrutura típica em projetos .NET

```
MinhaSolucao/
├── MinhaSolucao.Api/              # Controllers, Middleware, Program.cs
├── MinhaSolucao.Application/      # Services, DTOs, interfaces de repositório
├── MinhaSolucao.Domain/           # Entidades, Value Objects, exceções de domínio
└── MinhaSolucao.Infrastructure/   # EF Core DbContext, repositórios, RabbitMQ
```

### Exemplo prático

```csharp
// Api — só recebe HTTP e delega
[ApiController]
[Route("api/pedidos")]
public class PedidosController : ControllerBase
{
  private readonly IPedidoService _service;
  public PedidosController(IPedidoService service) => _service = service;

  [HttpPost]
  public async Task<ActionResult<PedidoDto>> Criar(CriarPedidoRequest request)
    => Ok(await _service.CriarAsync(request));
}

// Application — orquestra, não conhece EF nem HTTP
public class PedidoService : IPedidoService
{
  private readonly IPedidoRepository _repo;
  public async Task<PedidoDto> CriarAsync(CriarPedidoRequest request)
  {
    var pedido = Pedido.Criar(request.ClienteId, request.Itens);
    await _repo.AdicionarAsync(pedido);
    return PedidoDto.From(pedido);
  }
}

// Infrastructure — implementa persistência
public class PedidoRepository : IPedidoRepository
{
  private readonly AppDbContext _ctx;
  public async Task AdicionarAsync(Pedido pedido)
  {
    _ctx.Pedidos.Add(pedido);
    await _ctx.SaveChangesAsync();
  }
}
```

### Vantagens e limitações

| Vantagens | Limitações |
|-----------|------------|
| Simples de entender e onboard | Pode virar "camadas de passagem" (anêmico) |
| Boa separação de responsabilidades | Domínio às vezes fica fraco, lógica no Service |
| Funciona bem em CRUD e SaaS médio | Dificulta evolução para microserviços sem refatorar |

**Na entrevista:** cite que camadas são um **ponto de partida**, não um fim — o importante é **dependências apontarem para o domínio**, não para infraestrutura.

---

## Clean Architecture (Uncle Bob)

Objetivo: **regras de negócio no centro**, independentes de framework, banco e UI. A dependência sempre aponta **para dentro**.

```
         ┌──────────────────────────────────┐
         │  Frameworks & Drivers          │
         │  (ASP.NET, EF Core, RabbitMQ)   │
         │  ┌────────────────────────────┐  │
         │  │  Interface Adapters        │  │
         │  │  (Controllers, Repos)      │  │
         │  │  ┌──────────────────────┐  │  │
         │  │  │  Application         │  │  │
         │  │  │  (Use Cases)         │  │  │
         │  │  │  ┌────────────────┐  │  │  │
         │  │  │  │    Domain      │  │  │  │
         │  │  │  │  (Entities)    │  │  │  │
         │  │  │  └────────────────┘  │  │  │
         │  │  └──────────────────────┘  │  │
         │  └────────────────────────────┘  │
         └──────────────────────────────────┘
```

### Regra de ouro: Dependency Inversion

- **Domain** não referencia nada externo.
- **Application** define interfaces (`IRepository`, `IEmailSender`) e **casos de uso**.
- **Infrastructure** implementa essas interfaces.

```csharp
// Domain — sem referência a EF, ASP.NET, etc.
public class Pedido
{
  public Guid Id { get; private set; }
  public StatusPedido Status { get; private set; }

  public void Confirmar()
  {
    if (Status != StatusPedido.Rascunho)
      throw new DomainException("Pedido já confirmado.");
    Status = StatusPedido.Confirmado;
  }
}

// Application — caso de uso explícito
public class ConfirmarPedidoHandler
{
  private readonly IPedidoRepository _repo;
  private readonly IUnitOfWork _uow;

  public async Task HandleAsync(Guid pedidoId, CancellationToken ct)
  {
    var pedido = await _repo.ObterPorIdAsync(pedidoId, ct)
      ?? throw new NotFoundException("Pedido não encontrado.");
    pedido.Confirmar();
    await _uow.SaveChangesAsync(ct);
  }
}
```

### Clean Architecture vs Camadas tradicionais

| Camadas tradicionais | Clean Architecture |
|---------------------|-------------------|
| Service com lógica + orquestração | Use Case por operação de negócio |
| Entidade anêmica (só propriedades) | Entidade rica (comportamento no domínio) |
| Infra pode "vazar" para API | API só traduz HTTP ↔ Use Case |
| Acoplamento ao EF em Services | Repositório abstrai persistência |

### Registro de dependências (composition root)

Toda injeção acontece em **um único lugar** — geralmente `Program.cs` ou extensão `AddInfrastructure()`:

```csharp
// Program.cs
builder.Services.AddScoped<IConfirmarPedidoHandler, ConfirmarPedidoHandler>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddDbContext<AppDbContext>(...);
```

**Na entrevista:** "Clean Architecture não é sobre quantas pastas você tem — é sobre **quem depende de quem**. O domínio nunca depende do EF Core."

---

## DDD — Domain-Driven Design (nível prático)

Foco em modelar o software alinhado ao **linguagem ubíqua** do negócio (agronegócio, pedidos, safra, propriedade rural, etc.).

### Blocos de construção

| Conceito | O que é | Exemplo .NET |
|----------|---------|--------------|
| **Entity** | Objeto com identidade única | `Pedido` com `Guid Id` |
| **Value Object** | Objeto sem identidade, comparado por valor | `Dinheiro`, `Endereco`, `Cpf` |
| **Aggregate** | Cluster de entidades com uma raiz | `Pedido` + `ItemPedido` — só a raiz é acessada de fora |
| **Repository** | Abstração de persistência por agregado | `IPedidoRepository` |
| **Domain Service** | Lógica que não cabe numa entidade | `CalculadoraDeFrete` |
| **Domain Event** | Algo que aconteceu no domínio | `PedidoConfirmadoEvent` |

### Value Object imutável

```csharp
public record Dinheiro(decimal Valor, string Moeda = "BRL")
{
  public Dinheiro Somar(Dinheiro outro)
  {
    if (Moeda != outro.Moeda) throw new DomainException("Moedas diferentes.");
    return this with { Valor = Valor + outro.Valor };
  }
}
```

### Aggregate Root — proteja invariantes

```csharp
public class Pedido // Aggregate Root
{
  private readonly List<ItemPedido> _itens = new();
  public IReadOnlyCollection<ItemPedido> Itens => _itens.AsReadOnly();

  public void AdicionarItem(Produto produto, int quantidade)
  {
    if (Status != StatusPedido.Rascunho)
      throw new DomainException("Não é possível alterar pedido confirmado.");
    _itens.Add(new ItemPedido(produto.Id, quantidade, produto.Preco));
  }
}
// ItemPedido não é exposto para modificação direta de fora do agregado
```

### Bounded Context

Cada **contexto delimitado** tem seu próprio modelo. Exemplo em SaaS agro:

- **Contexto Comercial:** `Cliente`, `Pedido`, `Contrato`
- **Contexto Fazenda:** `Propriedade`, `Talhao`, `Safra`
- **Contexto Financeiro:** `Titulo`, `NotaFiscal`

Contextos se comunicam via **integração** (API, eventos), não compartilhando entidades acopladas.

### Domain Events + mensageria

```csharp
// Domain
public record PedidoConfirmadoEvent(Guid PedidoId, DateTime ConfirmadoEm);

// Application — após SaveChanges
await _eventBus.PublishAsync(new PedidoConfirmadoEvent(pedido.Id, DateTime.UtcNow));

// Outro serviço consome e atualiza estoque, envia e-mail, etc.
```

**Na entrevista (nível prático):** não precisa citar CQRS/Event Sourcing completos — mostre que entende **agregados**, **value objects**, **linguagem do domínio** e **separação por contexto**.

---

## SOLID na prática (.NET)

| Princípio | Significado | Exemplo ruim → bom |
|-----------|-------------|-------------------|
| **S** — Single Responsibility | Uma classe, um motivo para mudar | `PedidoService` que salva no banco E envia e-mail → separar `PedidoService` + `IEmailSender` |
| **O** — Open/Closed | Aberto para extensão, fechado para modificação | `switch` por tipo de pagamento → `IPagamentoStrategy` |
| **L** — Liskov Substitution | Subtipos substituem o base sem quebrar | `FakeRepository` nos testes substitui `PedidoRepository` |
| **I** — Interface Segregation | Interfaces pequenas e específicas | `IRepository` gigante → `IPedidoRepository`, `IClienteRepository` |
| **D** — Dependency Inversion | Dependa de abstrações | Controller depende de `IPedidoService`, não de `PedidoService` concreto |

```csharp
// DIP + ISP — padrão comum em APIs .NET
public interface IPedidoRepository
{
  Task<Pedido?> ObterPorIdAsync(Guid id, CancellationToken ct);
  Task AdicionarAsync(Pedido pedido, CancellationToken ct);
}

public class PedidosController : ControllerBase
{
  private readonly IConfirmarPedidoHandler _handler; // abstração, não implementação
}
```

---

## Padrões de projeto relevantes para backend

| Padrão | Uso em API .NET | Quando usar |
|--------|-----------------|-------------|
| **Repository** | `IProdutoRepository` | Abstrair EF Core / testes |
| **Unit of Work** | `DbContext` + `SaveChangesAsync` | Transações com múltiplos repositórios |
| **Factory** | `Pedido.Criar(...)` | Criação complexa com validação |
| **Strategy** | `IPoliticaDesconto` | Variações de regra (VIP, safra, volume) |
| **Mediator** | MediatR `IRequest<T>` | Desacoplar Controller de muitos handlers |
| **Decorator** | `LoggingHandler` em pipeline | Cross-cutting (log, cache, retry) |

### MediatR — Controller fino

```csharp
// Request
public record CriarPedidoCommand(Guid ClienteId, List<ItemDto> Itens) : IRequest<PedidoDto>;

// Handler
public class CriarPedidoHandler : IRequestHandler<CriarPedidoCommand, PedidoDto> { ... }

// Controller
[HttpPost]
public Task<PedidoDto> Criar(CriarPedidoCommand cmd, CancellationToken ct)
  => _mediator.Send(cmd, ct);
```

---

## APIs REST escaláveis

### Princípios REST que costumam cair na entrevista

- **Recursos** como substantivos: `/api/pedidos`, `/api/clientes/{id}`
- **Verbos HTTP** corretos: GET (ler), POST (criar), PUT/PATCH (atualizar), DELETE (remover)
- **Status codes** semânticos: 200, 201, 204, 400, 401, 403, 404, 409, 422, 500
- **Stateless** — cada request traz contexto (JWT, headers); servidor não guarda sessão
- **Versionamento** — `/api/v1/pedidos` ou header `Api-Version`

### Estrutura mínima em ASP.NET Core (.NET 6+)

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(...);

var app = builder.Build();
app.UseExceptionHandler(...);  // ProblemDetails
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");
app.Run();
```

### Respostas padronizadas (Problem Details — RFC 7807)

```csharp
// .NET 7+ — IExceptionHandler ou middleware
return Results.Problem(
  title: "Pedido inválido",
  statusCode: StatusCodes.Status422UnprocessableEntity,
  detail: "Quantidade deve ser maior que zero.");
```

### Performance e escala

| Técnica | O que faz |
|---------|-----------|
| **Paginação** | `?page=1&pageSize=20` — nunca retornar listas gigantes |
| **Projeção** | `.Select(x => new Dto { ... })` — só colunas necessárias |
| **AsNoTracking** | Leituras sem change tracker do EF |
| **Cache** | `IMemoryCache` / Redis para dados quentes |
| **Compressão** | `AddResponseCompression()` |
| **Rate limiting** | `.NET 7+` `AddRateLimiter()` |

---

## Camada de persistência — EF Core

Parte da **infraestrutura**, não do domínio.

### DbContext e configuração

```csharp
public class AppDbContext : DbContext
{
  public DbSet<Pedido> Pedidos => Set<Pedido>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
  }
}

// PedidoConfiguration.cs — Fluent API
public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
{
  public void Configure(EntityTypeBuilder<Pedido> builder)
  {
    builder.HasKey(p => p.Id);
    builder.Property(p => p.Status).HasConversion<string>();
    builder.HasIndex(p => p.ClienteId);
  }
}
```

### Migrations

```bash
dotnet ef migrations add AdicionaTabelaPedidos -p Infrastructure -s Api
dotnet ef database update -p Infrastructure -s Api
```

### Queries eficientes (SQL Server)

```csharp
// Ruim — N+1
var pedidos = await _ctx.Pedidos.ToListAsync();
foreach (var p in pedidos) { var itens = p.Itens; } // lazy load em loop

// Bom — Include ou projeção
var pedidos = await _ctx.Pedidos
  .AsNoTracking()
  .Include(p => p.Itens)
  .Where(p => p.ClienteId == clienteId)
  .ToListAsync();

// Melhor para listagem — só o necessário
var resumo = await _ctx.Pedidos
  .AsNoTracking()
  .Where(p => p.Status == StatusPedido.Confirmado)
  .Select(p => new PedidoResumoDto(p.Id, p.Total, p.CriadoEm))
  .ToListAsync();
```

### Transações e integridade

```csharp
await using var transaction = await _ctx.Database.BeginTransactionAsync();
try
{
  await _repo.AdicionarAsync(pedido);
  await _estoqueRepo.BaixarAsync(pedido.Itens);
  await _ctx.SaveChangesAsync();
  await transaction.CommitAsync();
}
catch
{
  await transaction.RollbackAsync();
  throw;
}
```

**Índices:** crie em colunas de `WHERE`, `JOIN` e `ORDER BY` frequentes. Evite índice em tudo — cada índice custa escrita.

---

## Mensageria e arquitetura assíncrona

Desacopla serviços: quem **publica** não espera quem **consome**. Escala processamento e aumenta resiliência.

```
┌─────────┐   publish    ┌──────────┐   consume   ┌─────────────┐
│ API     │ ───────────► │ RabbitMQ │ ──────────► │ Worker .NET │
│ Pedidos │              │  (fila)  │             │ Estoque     │
└─────────┘              └──────────┘             └─────────────┘
```

### Conceitos

| Termo | Significado |
|-------|-------------|
| **Producer** | Publica mensagem (API após criar pedido) |
| **Consumer** | Processa mensagem (worker de estoque) |
| **Queue** | Uma mensagem, um consumidor |
| **Exchange** | Roteia mensagens para filas (topic, direct, fanout) |
| **Dead Letter Queue** | Mensagens que falharam após N tentativas |

### Exemplo com MassTransit + RabbitMQ

```csharp
// Publicar
await _publishEndpoint.Publish(new PedidoCriadoMessage
{
  PedidoId = pedido.Id,
  Itens = pedido.Itens.Select(i => new ItemMessage(i.ProdutoId, i.Qtd)).ToList()
});

// Consumer
public class PedidoCriadoConsumer : IConsumer<PedidoCriadoMessage>
{
  public async Task Consume(ConsumeContext<PedidoCriadoMessage> context)
  {
    await _estoqueService.ReservarAsync(context.Message);
  }
}
```

### Garantias e idempotência

- Mensageria costuma ser **at-least-once** — a mesma mensagem pode chegar duas vezes.
- **Solução:** consumer idempotente (verificar se já processou pelo `MessageId` ou `PedidoId`).

**Na entrevista:** contraste **síncrono** (HTTP, acoplamento temporal) vs **assíncrono** (filas, desacoplamento, picos de carga).

---

## Sistemas distribuídos, alta disponibilidade e escala

### Conceitos que a vaga cita como diferencial

| Conceito | Definição rápida |
|----------|------------------|
| **Alta disponibilidade (HA)** | Sistema continua operando mesmo com falha de componentes (redundância, health checks) |
| **Escalabilidade horizontal** | Mais instâncias da API atrás de load balancer |
| **Escalabilidade vertical** | Mais CPU/RAM na mesma máquina (limite mais rápido) |
| **Consistência eventual** | Após evento, todos os serviços convergem — não é instantâneo |
| **CAP theorem** | Em partição de rede: escolha entre Consistência e Disponibilidade |

### Padrões comuns em SaaS .NET

```
                    ┌─────────────┐
   Clientes ──────► │ Load Balancer│
                    └──────┬──────┘
              ┌────────────┼────────────┐
              ▼            ▼            ▼
         [API Pod 1]  [API Pod 2]  [API Pod 3]   ← Docker/Kubernetes
              │            │            │
              └────────────┼────────────┘
                           ▼
                    [SQL Server]
                    [Redis Cache]
                    [RabbitMQ]
```

- **Health checks** — `/health` para o orchestrator remover instância doente
- **Retry + Circuit Breaker** — Polly em chamadas HTTP externas
- **Correlation ID** — rastrear request entre serviços nos logs

```csharp
// Polly — resiliência
builder.Services.AddHttpClient<IClienteExterno, ClienteExterno>()
  .AddStandardResilienceHandler(); // retry, timeout, circuit breaker (.NET 8+)
```

---

## Observabilidade

Três pilares: **logs**, **métricas**, **traces**.

| Pilar | Ferramenta comum (.NET / Azure) | O que registrar |
|-------|--------------------------------|-----------------|
| **Logs** | Serilog + Application Insights | Erros, contexto de negócio, CorrelationId |
| **Métricas** | Prometheus, App Insights | Request/s, latência p95, erros 5xx |
| **Traces** | OpenTelemetry | Fluxo completo API → DB → fila |

```csharp
// Serilog — structured logging
Log.Information("Pedido {PedidoId} confirmado para cliente {ClienteId}",
  pedido.Id, pedido.ClienteId);

// OpenTelemetry — Program.cs
builder.Services.AddOpenTelemetry()
  .WithTracing(t => t
    .AddAspNetCoreInstrumentation()
    .AddEntityFrameworkCoreInstrumentation()
    .AddOtlpExporter());
```

**Na entrevista:** observabilidade não é só "logar tudo" — é poder **responder perguntas em produção**: por que está lento? qual tenant? qual endpoint?

---

## Docker, Azure e CI/CD (contexto arquitetural)

### Docker — por que containerizar?

- Ambiente **reproduzível** (dev = staging = prod)
- Deploy de **API + Worker** como unidades independentes
- Base para Kubernetes / Azure Container Apps

```dockerfile
# Dockerfile multi-stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish Api/Api.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Api.dll"]
```

### Azure (diferencial da vaga)

| Serviço | Uso típico |
|---------|------------|
| **Azure App Service** | Hospedar API .NET |
| **Azure SQL** | Banco gerenciado |
| **Azure Service Bus** | Mensageria (alternativa ao RabbitMQ) |
| **Application Insights** | APM, logs, métricas |
| **Azure DevOps** | Pipelines CI/CD |

### Pipeline CI/CD (visão geral)

```
git push → build + test → análise estática → publish artefato → deploy staging → deploy prod
```

```yaml
# Azure DevOps — esboço
steps:
  - task: DotNetCoreCLI@2
    inputs:
      command: test
      projects: '**/*Tests.csproj'
  - task: DotNetCoreCLI@2
    inputs:
      command: publish
      publishWebProjects: true
```

---

## Como escolher a arquitetura? (trade-offs — muito cobrado)

A vaga pede explicitamente: **avaliar trade-offs e propor soluções escaláveis**.

| Cenário | Abordagem razoável |
|---------|-------------------|
| CRUD SaaS, time pequeno | Camadas + SOLID + testes |
| Regras de negócio complexas (agro, contratos) | Clean Architecture + DDD prático |
| Picos de processamento (relatórios, integrações) | API síncrona + fila + worker |
| Múltiplos produtos com modelos diferentes | Bounded Contexts, possivelmente microserviços depois |
| Começar simples, evoluir | Monólito modular bem separado > microserviços prematuros |

### Perguntas que você pode fazer na entrevista

- "O produto é monólito ou já tem serviços separados?"
- "Qual o volume de requests e tamanho do time?"
- "Já usam mensageria em produção? Qual broker?"

---

## Glossário rápido para a entrevista

| Termo | Frase de uma linha |
|-------|-------------------|
| **Clean Architecture** | Domínio no centro; frameworks são detalhes substituíveis |
| **DDD** | Modelar código com a linguagem do negócio; agregados protegem regras |
| **Camadas** | Separação API / aplicação / domínio / infra |
| **Repository** | Abstração de persistência por agregado |
| **CQRS** | Separar comandos (escrita) de queries (leitura) — citar se conhecer |
| **Event-driven** | Serviços reagem a eventos em vez de chamadas diretas |
| **Idempotência** | Executar N vezes = mesmo efeito que uma vez |
| **Problem Details** | Padrão de erro JSON em APIs modernas |
| **Composition Root** | Único lugar onde interfaces ligam a implementações |

---

## Checklist de revisão antes da entrevista

- [ ] Explicar diferença entre arquitetura em camadas e Clean Architecture
- [ ] Dar exemplo de Entity, Value Object e Aggregate no domínio agro/SaaS
- [ ] Mostrar como inverter dependência (interface no Application, impl na Infrastructure)
- [ ] Citar SOLID com exemplo real, não só definição
- [ ] Descrever fluxo REST: Controller → Handler/Service → Repository → EF
- [ ] Explicar quando usar fila em vez de HTTP síncrono
- [ ] Mencionar paginação, AsNoTracking, índices e N+1
- [ ] Saber o que é health check, structured log e CorrelationId
- [ ] Argumentar trade-off: monólito modular vs microserviços

---

## Leitura complementar

- [Microsoft — ASP.NET Core architecture](https://learn.microsoft.com/en-us/dotnet/architecture/)
- [eShopOnWeb](https://github.com/dotnet-architecture/eShopOnWeb) — referência Clean Architecture .NET
- [Clean Architecture (Robert C. Martin)](https://www.amazon.com/Clean-Architecture-Craftsmans-Software-Structure/dp/0134494164)
- [Domain-Driven Design (Eric Evans)](https://www.amazon.com/Domain-Driven-Design-Tackling-Complexity-Software/dp/0321125215)
