# Guia de preparação — Entrevista técnica Softplan (Integrações .NET)

Documento de apoio para a **Etapa 4: Excelência Técnica** (time de devs .NET + tech lead).

> **Contexto:** Após entrevista com RH (OK), ficou claro que a vaga é fortemente voltada a **integrações** entre produtos/sistemas (não tanto API greenfield), com ênfase em **arquitetura**, **observabilidade** e sustentação em produção. Este guia é **especulativo** — não é informação interna da Softplan — e foi montado com base na descrição oficial da vaga, no material de preparação do repositório e em boas práticas do ecossistema .NET.

**Prazo sugerido:** ~15 dias até a entrevista técnica.

---

## Índice

1. [O que provavelmente te espera nessa vaga](#1-o-que-provavelmente-te-espera-nessa-vaga)
2. [Mapa de tecnologias (foco integração)](#2-mapa-de-tecnologias-foco-integração)
3. [Plano prático de 15 dias](#3-plano-prático-de-15-dias)
4. [Cursos Udemy (4.6+ e 1000+ alunos)](#4-cursos-udemy-46-e-1000-alunos)
5. [Outros recursos (gratuitos e hands-on)](#5-outros-recursos-gratuitos-e-hands-on)
6. [O que demonstrar na entrevista técnica](#6-o-que-demonstrar-na-entrevista-técnica)
7. [Checklist final (véspera)](#7-checklist-final-véspera)
8. [Prioridade se tiver pouco tempo](#8-prioridade-se-tiver-pouco-tempo)

---

## 1. O que provavelmente te espera nessa vaga

### Perfil real (não “API greenfield”)

Pelo RH + descrição oficial, o papel é de **integrador/arquiteto de fluxo** mais do que de “criar CRUD do zero”:


| Dimensão               | O que tende a pesar                                                                     |
| ---------------------- | --------------------------------------------------------------------------------------- |
| **Dia a dia**          | Manter e evoluir integrações Softplan ↔ órgãos públicos/tribunais/sistemas externos     |
| **Estilo de trabalho** | Análise técnica, desenho de fluxo, implementação, testes, deploy, **sustentação**       |
| **Comunicação**        | REST, **SOAP** (legado gov), mensageria, contratos que evoluem sem quebrar consumidores |
| **Arquitetura**        | Serviços/microsserviços, assíncrono, rastreabilidade, falhas, **observabilidade**       |
| **Colaboração**        | Produto, QA, Infra — integração é trabalho de time                                      |
| **Sênior**             | Autonomia, priorização, propor melhorias, às vezes apoiar entrevistas                   |


### Stack provável (da vaga)

- **Core:** C#, ASP.NET Core (.NET 6+), APIs REST
- **Integração:** SOAP/WCF ou **CoreWCF**, HTTP clients resilientes, contratos OpenAPI
- **Assíncrono:** RabbitMQ (ou similar), workers, idempotência, DLQ
- **Dados:** SQL Server, Redis; Oracle como diferencial (sua experiência GOL)
- **Plataforma:** Git, Docker, CI/CD, testes unitários + integração com mocks
- **Ecossistema:** S3/MinIO, Elastic, Keycloak
- **Observabilidade:** logs estruturados, health checks, tracing distribuído (OpenTelemetry)
- **Gov/justiça:** alta criticidade, SLAs ruins de parceiros, LGPD, ambientes instáveis

### O que a entrevista técnica provavelmente avalia

Com base em relatos (Glassdoor), requisitos da vaga e roteiro de preparação:

1. **Histórias STAR** com integração real (GOL/Oracle, SERGET/RabbitMQ)
2. **Desenho de fluxo** — “como integraria sistema A com B?” (sync vs async, retry, idempotência)
3. **REST + contratos** — versionamento, breaking change, DTOs, erros padronizados
4. **SOAP/legado** — conceitos WCF/SOAP mesmo sem ter usado muito (muito comum em gov)
5. **Mensageria** — quando fila vs HTTP, DLQ, duplicata, consistência eventual
6. **Resiliência** — Polly (retry, circuit breaker, timeout), `IHttpClientFactory`
7. **Observabilidade** — correlation ID, traces, como debugar integração quebrada
8. **Fundamentos .NET** — DI, middleware, Background Services
9. **SOLID, refatoração, Docker, testes** — aparecem em relatos de entrevistas anteriores
10. **Menos provável:** algoritmos pesados ou live coding de estrutura de dados

O diferencial será **conectar teoria com produção**: “na SERGET fiz X quando a fila enfileirava / o parceiro caía”.

---

## 2. Mapa de tecnologias (foco integração)

### 2.1 Integração síncrona (REST/HTTP)

**Dominar conceitualmente:**

- `HttpClient` / `IHttpClientFactory` (não criar `HttpClient` por request)
- Timeouts, retry com backoff, circuit breaker (**Polly**)
- Contratos: OpenAPI/Swagger, versionamento (`/v1`, headers), Problem Details (RFC 7807)
- Autenticação: JWT, OAuth2, API keys, mTLS (contexto gov)

**Perguntas típicas:** “Parceiro retorna 503 intermitente — o que você faz?” “Como versiona sem quebrar 10 consumidores?”

### 2.2 SOAP / legado (muito relevante para Softplan)

WCF clássico **não existe** em .NET moderno; em produção gov costuma haver:

- **CoreWCF** (ponte temporária)
- **Cliente SOAP** gerado (Connected Services / `dotnet-svcutil`)
- Abstração: `IServicoTribunal` por trás de adapter SOAP

**Estudar:** envelope SOAP, WSDL, fault contracts, timeouts longos, certificados.

### 2.3 Integração assíncrona (RabbitMQ)

**Conceitos obrigatórios:**

- Exchange types: direct, topic, fanout
- Filas, bindings, **DLQ**, TTL, prefetch
- **Idempotência** (chave de negócio, deduplicação)
- **Outbox pattern** (DB + mensagem na mesma transação)
- **MassTransit** (abstração comum no ecossistema .NET)

**Quando usar fila vs HTTP:** volume, desacoplamento, picos, processamento longo, parceiro offline.

### 2.4 Arquitetura de integração

Padrões que vale saber **explicar** (nem todos precisam ter implementado):

- **Anti-Corruption Layer** (traduzir modelo do legado)
- **Strangler Fig** (migrar integração aos poucos)
- **Messaging Bridge** (ligar MSMQ ↔ RabbitMQ ↔ Service Bus)
- **Saga / compensação** (fluxos multi-etapa)
- **Claim Check** (payload grande no S3, referência na mensagem)
- **CQRS** (leitura vs escrita em integrações pesadas)

### 2.5 Observabilidade (destaque do RH)

**Três pilares:** logs, métricas, traces

**Na prática .NET:**

- **Serilog** + enrichers (CorrelationId, UserId, IntegrationName)
- **OpenTelemetry** → Jaeger/Zipkin/Elastic APM
- **Health checks** ASP.NET Core (`/health`, readiness)
- **SLI/SLO** básicos: taxa de erro de integração, latência p95, fila crescendo

**Frase forte na entrevista:** “Integração sem trace ID é debugging no escuro.”

### 2.6 Ecossistema da vaga


| Tecnologia    | Uso típico em integrações                        |
| ------------- | ------------------------------------------------ |
| **Redis**     | Cache de token/resposta, lock distribuído, dedup |
| **Elastic**   | Logs centralizados, busca de incidentes          |
| **S3/MinIO**  | Anexos, payloads grandes, auditoria              |
| **Keycloak**  | SSO, tokens entre sistemas                       |
| **Docker/CI** | Testar integração local com compose              |


---

## 3. Plano prático de 15 dias


| Dias      | Foco               | Entrega hands-on                                                         |
| --------- | ------------------ | ------------------------------------------------------------------------ |
| **1–2**   | REST resiliente    | API + client com Polly + correlation ID nos logs                         |
| **3–4**   | RabbitMQ           | Produtor/consumidor + DLQ + retry; desenhar fluxo no papel               |
| **5–6**   | Contratos & testes | OpenAPI + teste de integração com **WireMock** ou Testcontainers         |
| **7**     | SOAP               | Ler WSDL + consumir mock SOAP (ou CoreWCF hello world)                   |
| **8–9**   | Observabilidade    | OpenTelemetry + trace entre API → fila → worker                          |
| **10**    | Padrões            | Outbox + idempotência (artigo + pseudo-implementação)                    |
| **11–12** | Revisão STAR       | 5 histórias (GOL Oracle, SERGET RabbitMQ, falha, refatoração, incidente) |
| **13**    | System design      | 2 cenários: “tribunal envia processo” / “fila com pico”                  |
| **14**    | Mock interview     | Explicar em voz alta sem slides                                          |
| **15**    | Descanso leve      | Revisar checklist + perguntas para o lead                                |


### Projeto mínimo recomendado: `IntegrationLab`

API .NET 8 + RabbitMQ (Docker Compose) + worker + Polly chamando API fake + OpenTelemetry + Serilog com `CorrelationId`.

Isso cobre a maior parte do que a vaga pede e dá material concreto para falar na entrevista.

---

## 4. Cursos Udemy (4.6+ e 1000+ alunos)

> Confira rating e número de alunos na página do Udemy antes de comprar (mudam com o tempo).

### Prioridade alta (cumprem critério e alinham à vaga)


| Curso                                                                                                                                                      | Rating ~ | Alunos ~ | Por que                                                                               |
| ---------------------------------------------------------------------------------------------------------------------------------------------------------- | -------- | -------- | ------------------------------------------------------------------------------------- |
| [.NET 8 Microservices: DDD, CQRS, Vertical/Clean Architecture](https://www.udemy.com/course/microservices-architecture-and-implementation-on-dotnet/)      | 4.6      | 48k+     | RabbitMQ, MassTransit, Redis, YARP, gRPC — **melhor custo/benefício** para integração |
| [Build ASP.NET Core Web API - Scratch To Finish (.NET8)](https://www.udemy.com/course/build-rest-apis-with-aspnet-core-web-api-entity-framework/)          | 4.6      | 11k+     | REST, EF, auth JWT — base sólida de API                                               |
| [.NET 7 Web API & Entity Framework Jumpstart](https://www.udemy.com/course/net-core-31-web-api-entity-framework-core-jumpstart/)                           | 4.7      | 20k+     | REST rápido e direto                                                                  |
| [.NET Microservices with Azure DevOps & AKS](https://www.udemy.com/course/dot-net-microservices-ecommerce-project-azure-devops-kubernetes-aks/)            | 4.7      | 9k+      | Polly, RabbitMQ, Redis, CI/CD                                                         |
| [Build a Microservices app with .Net and NextJS from scratch](https://www.udemy.com/course/build-a-microservices-app-with-dotnet-and-nextjs-from-scratch/) | 4.7      | 5.7k+    | RabbitMQ + MassTransit + testes                                                       |


### Complementares (observabilidade / resiliência)


| Curso                                                                                                                                                 | Rating ~ | Nota                                                                    |
| ----------------------------------------------------------------------------------------------------------------------------------------------------- | -------- | ----------------------------------------------------------------------- |
| [OpenTelemetry Foundations: Hands-On Guide to Observability](https://www.udemy.com/course/opentelemetry-foundations/)                                 | 4.6      | Conceitos OTel (verificar contagem de alunos na Udemy)                  |
| [Microservices Observability, Resilience, Monitoring on .Net](https://www.udemy.com/course/microservices-observability-resilience-monitoring-on-net/) | ~4.4     | **Abaixo de 4.6**, mas muito alinhado: Polly + Serilog + Elastic + OTel |


### SOAP (abaixo de 4.6, mas úteis para contexto gov)


| Curso                                                                                                                             | Rating ~          |
| --------------------------------------------------------------------------------------------------------------------------------- | ----------------- |
| [REST WCF Service for Web Applications in ASP.NET](https://www.udemy.com/course/rest-wcf-service-for-web-applications-in-aspnet/) | 4.5 (~1k ratings) |
| [Mastering WCF 4.0 From Scratch](https://www.udemy.com/course/masterwcf/)                                                         | 4.5               |


Para SOAP em .NET moderno, complemente com documentação **CoreWCF** e artigos de migração WCF → REST/gRPC.

### Evitar como foco principal para esta vaga

- Cursos muito focados em **Angular/front** (ex.: REST + Angular) — úteis, mas desvio de tempo.
- Cursos só de **DDD/Clean** sem mensageria — bom como cultura, fraco para integração.

---

## 5. Outros recursos (gratuitos e hands-on)

### Documentação e guias Microsoft

- [Asynchronous message-based communication](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/architect-microservice-container-applications/asynchronous-message-based-communication)
- [Integration events](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/multi-container-microservice-net-applications/integration-event-based-microservice-communications)
- [Messaging Bridge pattern](https://learn.microsoft.com/en-us/azure/architecture/patterns/messaging-bridge)
- [Cloud Design Patterns – Messaging](https://learn.microsoft.com/en-us/azure/architecture/patterns/category/messaging)

### Repositórios hands-on

- [aspnetrun/microservices](https://github.com/aspnetrun/aspnetrun) — ecossistema do curso Mehmet Ozkaya (RabbitMQ + Polly)
- [dotnet/eShopOnContainers](https://github.com/dotnet/eShopOnContainers) — event bus, integração entre serviços
- [mehdihadeli/food-delivery-microservices](https://github.com/mehdihadeli/food-delivery-microservices) — MassTransit + Aspire

### Vídeos / trilhas

- [Messaging Patterns for Modern Solutions](https://particular.net/videos/messaging-patterns-modern-solutions) (Particular/NServiceBus) — Outbox, idempotência
- [From Zero to Hero: OpenTelemetry in .NET](https://dometrain.com/course/from-zero-to-hero-open-telemetry-in-net/) (Dometrain — pago, focado .NET)

### Leitura curta de alto impacto

- [Polly + Retry/Circuit Breaker (aspnetrun/Medium)](https://medium.com/aspnetrun/microservices-resilience-and-fault-tolerance-with-applying-retry-and-circuit-breaker-patterns-c32e518db990)
- [MassTransit — documentação](https://masstransit.io/documentation/configuration)

### Documentos neste repositório

- `DescricaoVaga.md` — requisitos oficiais
- `PreparacaoEntrevista_PerguntasParaResponder.md` — roteiro de perguntas (seções 5–7 e 11 são prioridade)
- `EntrevistaRH_RespostasProntas.md` — histórias STAR e elevator pitch

---

## 6. O que demonstrar na entrevista técnica

### 6.1 Abrir com arquitetura de integração (2 min)

Desenhe no quadro/virtual:

```
[Sistema A] --REST/SOAP--> [Adapter/ACL] --> [API .NET] --> [RabbitMQ] --> [Worker]
                                |                    |
                           [SQL Server]          [Redis cache]
                                |
                         CorrelationId em todos os logs/traces
```

### 6.2 Cinco histórias que você já tem (reforçar)

Do seu material: **GOL + Oracle**, **SERGET + RabbitMQ + Redis + Background Services**, migração Framework → .NET 8, OCR/IA assíncrono, otimização SQL (~80%).

Para cada uma, tenha pronto:

- Contrato (quem consumia o quê)
- Falha que aconteceu ou poderia acontecer
- Como monitorou / descobriu o problema
- Decisão arquitetural (por que fila, por que retry, etc.)

### 6.3 Perguntas técnicas — respostas-modelo curtas

**“HTTP ou mensageria?”**  
HTTP quando preciso resposta imediata e acoplamento aceitável; mensageria quando há pico, processamento longo, parceiro instável ou vários consumidores.

**“Como evita processar a mesma mensagem duas vezes?”**  
Idempotency key na tabela, `INSERT` com unique constraint, ou store de mensagens processadas no Redis.

**“Parceiro SOAP lento e instável?”**  
Timeout agressivo no client, circuit breaker, fila interna para não bloquear thread pool, cache de respostas idempotentes quando fizer sentido.

**“Breaking change em API pública?”**  
Versionar (`/v2`), período de deprecação, contrato testado em CI, feature flag por consumidor.

**“Incidente: integração parou — como investiga?”**  
Health check → métrica de fila → logs por CorrelationId → trace distribuído → último deploy/contrato alterado → contato com parceiro se externo.

### 6.4 Lacunas honestas (e como contornar)

Se integração ainda não é seu conforto máximo:

- “Tenho produção em RabbitMQ e REST; estou aprofundando SOAP/CoreWCF e padrões Outbox/Saga com lab X.”
- Mostre **curiosidade + método de aprendizado**, não fingir domínio em WSDL.

### 6.5 Perguntas para fazer ao tech lead

- Qual stack de observabilidade o time usa hoje (Elastic, APM, Grafana)?
- Quais integrações são REST vs SOAP vs mensageria na prática?
- Como funciona on-call / sustentação de integrações críticas com tribunais?

---

## 7. Checklist final (véspera)

- 5 histórias STAR com números (latência, volume, % melhoria)
- Explicar fluxo SERGET e GOL em 3 min cada
- Desenhar sync vs async sem hesitar
- Polly: retry + circuit breaker + quando **não** retentar (POST não idempotente)
- RabbitMQ: exchange, fila, DLQ, prefetch
- OpenTelemetry: trace entre serviços
- SOAP vs REST: 1 min de diferença prática
- 3 perguntas para o tech lead preparadas

---

## 8. Prioridade se tiver pouco tempo

Se só der para **uma** trilha em 15 dias:

1. **Lab IntegrationLab** (REST + RabbitMQ + Polly + logs com CorrelationId)
2. Módulos de mensageria do curso **.NET 8 Microservices** (Mehmet Ozkaya)
3. Revisar seções 5–7 e 11 do `PreparacaoEntrevista_PerguntasParaResponder.md`
4. Gravar você mesmo explicando um caso GOL ou SERGET

---

## Resumo

Você **não está começando do zero**: RabbitMQ, Redis, Oracle, APIs em produção são exatamente o que a vaga descreve. O gap é mais **vocabulário de arquitetura de integração** (padrões, SOAP gov, observabilidade formal) e **confiança para narrar** — isso se treina em ~15 dias com lab + histórias STAR.

---

*Última atualização: maio/2026*