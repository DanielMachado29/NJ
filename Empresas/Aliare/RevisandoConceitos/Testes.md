# Testes Automatizados — Resumo Prático (.NET / xUnit)

## Por que testar?

- **Confiança** para refatorar sem medo de quebrar comportamento existente.
- **Documentação viva** do que o código deve fazer.
- **Feedback rápido** — erro detectado em segundos, não em produção.
- **Design melhor** — código testável tende a ser mais desacoplado (SOLID).

---

## Pirâmide de testes

```
        /\
       /E2E\        poucos — lentos, frágeis, caros
      /------\
     /Integração\   quantidade média
    /------------\
   /  Unitários   \  muitos — rápidos, baratos, isolados
  /----------------\
```

| Tipo | O que testa | Velocidade | Exemplo |
|------|-------------|------------|---------|
| **Unitário** | Uma unidade isolada (classe/método) | Muito rápido | `Calculadora.Somar(2, 3)` |
| **Integração** | Interação entre componentes reais | Médio | API + banco em memória |
| **E2E** | Fluxo completo do usuário | Lento | Playwright, Selenium |

**Regra prática:** muitos unitários, alguns de integração, poucos E2E.

---

## Anatomia de um teste — padrão AAA

```csharp
[Fact]
public void Desconto_DeveAplicar10Porcento_QuandoClienteVip()
{
    // Arrange — preparar dados e dependências
    var cliente = new Cliente { Vip = true };
    var servico = new DescontoService();

    // Act — executar o comportamento
    var resultado = servico.Calcular(100m, cliente);

    // Assert — verificar o resultado
    Assert.Equal(90m, resultado);
}
```

---

## TDD — Test-Driven Development

Ciclo **Red → Green → Refactor**:

1. **Red** — escreva um teste que falha (comportamento ainda não existe).
2. **Green** — escreva o mínimo de código para o teste passar.
3. **Refactor** — melhore o código mantendo os testes verdes.

```csharp
// 1. RED — teste primeiro
[Fact]
public void ValidarEmail_DeveRetornarFalse_QuandoEmailInvalido()
{
    var validador = new ValidadorEmail();
    Assert.False(validador.EhValido("email-sem-arroba"));
}

// 2. GREEN — implementação mínima
public class ValidadorEmail
{
    public bool EhValido(string email) => email.Contains('@');
}

// 3. REFACTOR — regex, tratamento de null, etc.
```

### Quando usar TDD

| Bom para | Evitar quando |
|----------|---------------|
| Regras de negócio claras | UI/UX exploratório |
| Algoritmos, validações | Requisitos muito voláteis |
| APIs com contrato definido | Spike/protótipo descartável |

**Dica:** TDD não é "sempre escrever teste antes". É usar o teste como guia de design quando faz sentido.

---

## Dublês de teste (Test Doubles)

Termo guarda-chuva para objetos que substituem dependências reais em testes.

### Resumo rápido

| Dublê | O que faz | Quando usar |
|-------|-----------|-------------|
| **Dummy** | Só preenche parâmetro, nunca é usado | Compilar código que exige um objeto |
| **Stub** | Retorna respostas fixas | Isolar unidade de dependência externa |
| **Spy** | Stub que **registra** como foi chamado | Verificar interação sem rigidez de mock |
| **Mock** | Verifica **comportamento esperado** (chamadas, args) | Garantir que colaborador foi usado corretamente |
| **Fake** | Implementação simplificada mas funcional | Repositório em memória, `FakeHttpClient` |

### Dummy

```csharp
// Interface exigida, mas o teste não depende dela
public class LoggerDummy : ILogger
{
    public void Log(string msg) { } // não faz nada
}

var sut = new PedidoService(new LoggerDummy());
```

### Stub

```csharp
// Retorna valor fixo — você não se importa COMO foi chamado
var repoStub = new Mock<IPedidoRepository>();
repoStub.Setup(r => r.BuscarPorId(1))
        .Returns(new Pedido { Id = 1, Total = 100m });

var service = new PedidoService(repoStub.Object);
var pedido = service.ObterTotal(1);

Assert.Equal(100m, pedido);
```

### Spy

```csharp
// Registra chamadas para inspeção depois
var chamadas = new List<string>();
var notificador = new NotificadorSpy(chamadas);

var service = new PedidoService(notificador);
service.Finalizar(pedido);

Assert.Contains("pedido-finalizado", chamadas);
```

### Mock

```csharp
// Verifica COMPORTAMENTO — quantas vezes, com quais args
var emailMock = new Mock<IEmailService>();

var service = new PedidoService(emailMock.Object);
service.Finalizar(pedido);

emailMock.Verify(
    e => e.Enviar(pedido.ClienteEmail, It.IsAny<string>()),
    Times.Once);
```

### Fake

```csharp
// Implementação real, mas leve — útil em testes de integração leve
public class PedidoRepositoryFake : IPedidoRepository
{
    private readonly List<Pedido> _pedidos = new();

    public void Salvar(Pedido p) => _pedidos.Add(p);
    public Pedido? BuscarPorId(int id) => _pedidos.FirstOrDefault(p => p.Id == id);
}
```

### Quando usar o quê — regra prática

- **Prefira stub** quando só precisa controlar retorno.
- **Use mock** quando o **efeito colateral** (enviar email, publicar evento) é o que importa.
- **Use fake** quando a lógica do colaborador importa (ex.: repositório em memória).
- **Evite mockar tudo** — se a classe é um DTO ou value object, use o objeto real.

> **Mock vs Stub (debate clássico):** mocks testam *comportamento*; stubs testam *estado/resultado*. Prefira testar resultado quando possível — testes ficam menos frágeis.

---

## xUnit — biblioteca de testes para .NET

A mais usada no ecossistema .NET moderno. Inspirada no modelo JUnit/NUnit, mas com API mais limpa.

### Setup básico

```bash
dotnet new xunit -n MeuProjeto.Tests
dotnet add MeuProjeto.Tests reference MeuProjeto
dotnet add MeuProjeto.Tests package Moq          # mocks
dotnet add MeuProjeto.Tests package FluentAssertions  # assertions legíveis (opcional)
```

### Atributos principais

| Atributo | Uso |
|----------|-----|
| `[Fact]` | Teste simples, sem parâmetros |
| `[Theory]` + `[InlineData]` | Teste parametrizado |
| `[Trait("Category", "Unit")]` | Categorizar/filtrar testes |
| `IClassFixture<T>` | Setup compartilhado por classe |
| `ICollectionFixture<T>` | Setup compartilhado entre classes |

```csharp
[Theory]
[InlineData(2, 3, 5)]
[InlineData(-1, 1, 0)]
[InlineData(0, 0, 0)]
public void Somar_DeveRetornarResultadoCorreto(int a, int b, int esperado)
{
    var calc = new Calculadora();
    Assert.Equal(esperado, calc.Somar(a, b));
}
```

### Assertions nativas vs FluentAssertions

```csharp
// xUnit nativo
Assert.Equal(10, resultado);
Assert.True(cliente.Ativo);
Assert.Throws<ArgumentException>(() => servico.Processar(null));

// FluentAssertions (mais legível)
resultado.Should().Be(10);
cliente.Ativo.Should().BeTrue();
servico.Invoking(s => s.Processar(null)).Should().Throw<ArgumentException>();
```

### Organização de testes

```
MeuProjeto/
├── src/
│   └── MeuProjeto/
└── tests/
    └── MeuProjeto.Tests/
        ├── Unit/
        │   ├── Services/
        │   └── Domain/
        └── Integration/
            └── Api/
```

### Convenção de nomes

```
MetodoTestado_Cenario_ResultadoEsperado
```

Exemplos:
- `CriarPedido_SemEstoque_DeveLancarExcecao`
- `CalcularFrete_ParaCepInvalido_DeveRetornarZero`

---

## Moq — biblioteca de mocks mais usada com xUnit

```csharp
public class PedidoServiceTests
{
    private readonly Mock<IPedidoRepository> _repoMock;
    private readonly Mock<IEmailService> _emailMock;
    private readonly PedidoService _sut; // System Under Test

    public PedidoServiceTests()
    {
        _repoMock = new Mock<IPedidoRepository>();
        _emailMock = new Mock<IEmailService>();
        _sut = new PedidoService(_repoMock.Object, _emailMock.Object);
    }

    [Fact]
    public async Task Finalizar_DeveSalvarEPublicarEvento()
    {
        var pedido = new Pedido { Id = 1 };
        _repoMock.Setup(r => r.BuscarPorId(1)).ReturnsAsync(pedido);

        await _sut.FinalizarAsync(1);

        _repoMock.Verify(r => r.Salvar(It.Is<Pedido>(p => p.Status == Status.Finalizado)), Times.Once);
        _emailMock.Verify(e => e.EnviarConfirmacao(pedido.ClienteEmail), Times.Once);
    }
}
```

**Setup comuns:**

```csharp
// Retorno async
mock.Setup(x => x.BuscarAsync(It.IsAny<int>())).ReturnsAsync(entidade);

// Lançar exceção
mock.Setup(x => x.Salvar(It.IsAny<Pedido>())).Throws<DbException>();

// Callback — executar ação quando chamado
mock.Setup(x => x.Log(It.IsAny<string>())).Callback<string>(msg => _logs.Add(msg));

// Qualquer argumento
mock.Setup(x => x.Processar(It.IsAny<string>())).Returns(true);
```

---

## Testes de integração em APIs .NET

Usa `WebApplicationFactory` para subir a API em memória.

```csharp
public class PedidosApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public PedidosApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Substituir DbContext por banco em memória
                services.RemoveAll<DbContextOptions<AppDbContext>>();
                services.AddDbContext<AppDbContext>(opt =>
                    opt.UseInMemoryDatabase("TestDb"));
            });
        }).CreateClient();
    }

    [Fact]
    public async Task GetPedido_DeveRetornar200_QuandoExiste()
    {
        var response = await _client.GetAsync("/api/pedidos/1");
        response.EnsureSuccessStatusCode();
        var pedido = await response.Content.ReadFromJsonAsync<PedidoDto>();
        Assert.NotNull(pedido);
    }
}
```

**Pacotes úteis:**
- `Microsoft.AspNetCore.Mvc.Testing` — `WebApplicationFactory`
- `Microsoft.EntityFrameworkCore.InMemory` — banco em memória
- `Testcontainers` — containers Docker reais (SQL Server, RabbitMQ) para integração mais fiel

---

## O que testar e o que NÃO testar

### Teste

- Regras de negócio (domínio)
- Validações e edge cases
- Mapeamentos complexos
- Comportamento de serviços com dependências mockadas
- Endpoints (integração) com cenários principais

### Não teste (ou teste pouco)

- Propriedades get/set triviais
- Código gerado (migrations EF, DTOs puros)
- Frameworks de terceiros (confie que o EF funciona)
- Detalhes de implementação privados — teste o comportamento público

---

## Boas práticas

### 1. Um conceito por teste
Cada `[Fact]` deve verificar **uma coisa**. Se falhar, você sabe exatamente o que quebrou.

### 2. Testes independentes
Não compartilhe estado mutável entre testes. Ordem de execução não deve importar.

### 3. Teste comportamento, não implementação
```csharp
// ❌ Frágil — acoplado à implementação interna
mock.Verify(x => x.ChamarMetodoInterno(), Times.Once);

// ✅ Melhor — verifica resultado/efeito observável
Assert.Equal(Status.Finalizado, pedido.Status);
```

### 4. Dados de teste claros
Use **Object Mother** ou **Bogus** para fixtures complexas.

```csharp
public static class PedidoBuilder
{
    public static Pedido VipComTotal(decimal total) => new()
    {
        Cliente = new Cliente { Vip = true },
        Total = total
    };
}
```

### 5. Evite lógica nos testes
Sem `if`, `for` complexo ou `try/catch` no corpo do teste. Use `[Theory]` para variações.

### 6. Mantenha testes rápidos
Unitários < 100ms cada. Se um teste unitário demora, provavelmente não é unitário (banco, rede, arquivo).

### 7. Não repita setup — mas não abstraia demais
Construtor da classe de teste para setup comum. Evite hierarquias de base classes complexas.

### 8. Teste casos de borda
- `null`, string vazia, lista vazia
- Valores limites (0, -1, `int.MaxValue`)
- Concorrência (quando relevante)

### 9. CI/CD
```bash
dotnet test --configuration Release --collect:"XPlat Code Coverage"
```
Rode testes em todo PR. Meta comum: **> 80%** em domínio/regras de negócio (não obsessão por 100% global).

---

## Padrões úteis

### SUT (System Under Test)
Nomeie `_sut` para a classe sendo testada — deixa claro o foco.

### Test Data Builder
```csharp
var pedido = new PedidoBuilder()
    .ComClienteVip()
    .ComItens(3)
    .Build();
```

### Custom Assertions
```csharp
public static void DeveSerPedidoValido(this Pedido pedido)
{
    Assert.NotNull(pedido);
    Assert.True(pedido.Total > 0);
    Assert.NotEmpty(pedido.Itens);
}
```

---

## Checklist antes de abrir PR

- [ ] Testes unitários para regras de negócio novas/alteradas
- [ ] Teste de integração para endpoints novos (happy path + erro)
- [ ] Nomes descritivos (`Metodo_Cenario_Resultado`)
- [ ] Sem dependência de ordem entre testes
- [ ] Sem sleeps/`Thread.Sleep` — use mocks ou `TaskCompletionSource`
- [ ] `dotnet test` passando localmente

---

## Referências rápidas

| Ferramenta | Pacote NuGet |
|------------|--------------|
| Framework de testes | `xunit` + `xunit.runner.visualstudio` |
| Mocks | `Moq` ou `NSubstitute` |
| Assertions fluentes | `FluentAssertions` |
| Dados fake | `Bogus` |
| API testing | `Microsoft.AspNetCore.Mvc.Testing` |
| Coverage | `coverlet.collector` |
| Containers | `Testcontainers` |

### Comandos úteis

```bash
dotnet test                          # roda todos os testes
dotnet test --filter "Category=Unit" # filtra por trait
dotnet test --logger "console;verbosity=detailed"
dotnet test /p:CollectCoverage=true  # com cobertura
```

---

## Resumo mental para entrevista

1. **Pirâmide:** muitos unitários, poucos E2E.
2. **AAA:** Arrange, Act, Assert.
3. **TDD:** Red → Green → Refactor quando a regra é clara.
4. **Dublês:** Dummy (preenche), Stub (retorna), Spy (registra), Mock (verifica), Fake (implementação leve).
5. **Prefira** testar resultado sobre verificar chamadas internas.
6. **xUnit:** `[Fact]`, `[Theory]`, `IClassFixture` para setup.
7. **Moq:** `Setup` para comportamento, `Verify` para verificar chamadas.
8. **Integração:** `WebApplicationFactory` + banco em memória ou Testcontainers.
