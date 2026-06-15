# Preparação para Entrevista — Pessoa Desenvolvedora Backend .NET Sênior (Remoto)

Use este documento para estruturar suas respostas antes das entrevistas. Sempre que possível, responda usando o método **STAR** (Situação → Tarefa → Ação → Resultado), conforme orientado pela Softplan.

> **Dica:** Para cada pergunta, escreva uma resposta objetiva (3–5 minutos faladas). Priorize experiências da SERGET, CS Global IT e ATIVA GR que tenham relação direta com integrações, .NET, mensageria e sistemas de alta criticidade.

---

## Índice

1. [Apresentação e storytelling](#1-apresentação-e-storytelling)
2. [Motivação e fit com a Softplan](#2-motivação-e-fit-com-a-softplan)
3. [Comportamentais e situacionais (RH)](#3-comportamentais-e-situacionais-rh)
4. [.NET, C# e ASP.NET Core](#4-net-c-e-aspnet-core)
5. [Integrações (REST, SOAP e contratos)](#5-integrações-rest-soap-e-contratos)
6. [Arquitetura distribuída, microsserviços e resiliência](#6-arquitetura-distribuída-microsserviços-e-resiliência)
7. [Mensageria e processamento assíncrono](#7-mensageria-e-processamento-assíncrono)
8. [Bancos de dados, cache e armazenamento](#8-bancos-de-dados-cache-e-armazenamento)
9. [Testes, qualidade e boas práticas](#9-testes-qualidade-e-boas-práticas)
10. [DevOps, containers e CI/CD](#10-devops-containers-e-cicd)
11. [Observabilidade, troubleshooting e sustentação](#11-observabilidade-troubleshooting-e-sustentação)
12. [Segurança em integrações](#12-segurança-em-integrações)
13. [Ecossistema distribuído (S3, Elastic, Keycloak)](#13-ecossistema-distribuído-s3-elastic-keycloak)
14. [IA no dia a dia de desenvolvimento](#14-ia-no-dia-a-dia-de-desenvolvimento)
15. [Contexto governamental e alta criticidade](#15-contexto-governamental-e-alta-criticidade)
16. [Liderança técnica e colaboração](#16-liderança-técnica-e-colaboração)
17. [Perguntas reportadas no Glassdoor (Softplan)](#17-perguntas-reportadas-no-glassdoor-softplan)
18. [Perguntas para fazer aos entrevistadores](#18-perguntas-para-fazer-aos-entrevistadores)
19. [Checklist final antes da entrevista](#19-checklist-final-antes-da-entrevista)

---

## 1. Apresentação e storytelling

### Sobre você
- Fale sobre você em 2 minutos: quem é, formação, trajetória e o que faz hoje.
- Qual região você mora? Como isso se encaixa no modelo **Anywhere Office** (100% remoto)?
- Quais são suas **3 principais habilidades** técnicas e **2 diferenciais** comportamentais?

### Trajetória e projetos
- Conte sua trajetória profissional em ordem cronológica, destacando a evolução de júnior/pleno para sênior.
- Quais projetos da sua carreira você mais se orgulha? Por quê?
- Descreva um projeto em que você atuou **ponta a ponta** (da análise ao deploy/sustentação).
- Quais experiências anteriores, conhecimentos e projetos você destaca como mais relevantes para **esta vaga específica**?

### Storytelling de práticas
- Crie um storytelling sobre suas **práticas de engenharia**: como você escreve código, revisa, testa, faz deploy e sustenta em produção.
- Como você descreveria seu estilo de trabalho em uma frase que um colega de time usaria?

---

## 2. Motivação e fit com a Softplan

- O que você sabe sobre a Softplan? Por que quer trabalhar aqui?
- O que te atrai em uma empresa de **software para o setor público** (GovTech)?
- Como você se conecta com o propósito de impactar a vida de milhões de pessoas através de tecnologia?
- Por que esta vaga de **Backend .NET Sênior com foco em integrações** e não outra posição?
- O que você espera encontrar na Softplan nos próximos 2–3 anos de carreira?
- Quais resultados seus são **aderentes** aos requisitos da vaga? (Liste com exemplos concretos.)

---

## 3. Comportamentais e situacionais (RH)

> Responda sempre com STAR. Tenha pelo menos 5 histórias prontas reutilizáveis.

### Casos de sucesso e desafios
- Conte sobre um **caso de sucesso** no seu último emprego.
- Descreva um **desafio técnico complexo** que você enfrentou. Como resolveu?
- Conte sobre uma situação em que você **errou** ou causou um problema. O que aprendeu?
- Já precisou entregar algo com **prazo apertado** e escopo incerto? Como priorizou?
- Descreva um conflito com colega, PO ou stakeholder. Como conduziu a resolução?

### Autonomia e ambiguidade
- Dê um exemplo de quando você teve que tomar uma **decisão técnica** sem orientação clara.
- Como você lida com **ambiguidade** em requisitos de integração com sistemas externos?
- Conte sobre uma vez em que você teve que **dizer "não"** ou repriorizar demandas.

### Comunicação e colaboração
- Como você explica um problema técnico complexo para alguém não técnico?
- Como você colabora com times de Produto, QA e Infraestrutura?
- Já participou de entrevistas técnicas ou avaliação de candidatos? Como foi?

### Cultura e valores
- O que é importante para você em um time de engenharia?
- Como você lida com **feedback** recebido em code review?
- Como você compartilha conhecimento com o time?

---

## 4. .NET, C# e ASP.NET Core

### Fundamentos e experiência
- Qual sua experiência com **C# e ASP.NET Core** (.NET 6+)? Quais versões você já usou em produção?
- Descreva a arquitetura de uma API .NET que você desenvolveu recentemente (camadas, dependências, fluxo de request).
- Qual a diferença prática entre **.NET Framework** e **.NET 8**? Conte sobre sua experiência migrando módulos legados.

### ASP.NET Core na prática
- Como você estrutura um projeto ASP.NET Core para separar responsabilidades (Controllers, Services, Repositories, etc.)?
- Como funciona o **pipeline de middleware** no ASP.NET Core? Quais middlewares você já implementou ou configurou?
- Como você implementa **injeção de dependência** e por que isso importa?
- O que são **Background Services** e **Hosted Services**? Dê um exemplo real do seu trabalho.

### Performance e qualidade de código
- Como você garante **performance** em APIs .NET de alto volume?
- Quais ferramentas ou técnicas você usa para identificar gargalos (profiling, logging, APM)?
- Como você trata **validações** e **erros centralizados** em APIs?

---

## 5. Integrações (REST, SOAP e contratos)

> **Tema central da vaga.** Prepare respostas detalhadas aqui.

### REST
- Descreva uma integração REST que você construiu ou manteve. Qual era o contrato? Quem consumia?
- Como você modela **contratos de API** (versionamento, DTOs, documentação — Swagger/OpenAPI)?
- Como você lida com **breaking changes** em APIs consumidas por múltiplos sistemas?
- Quais estratégias você usa para **retry**, **timeout** e **circuit breaker** em chamadas HTTP?

### SOAP e legado
- Qual sua experiência com **SOAP**? Já integrou com sistemas governamentais ou legados via SOAP?
- Como você abstrai a complexidade de um serviço SOAP para o restante da aplicação?
- Já precisou manter integrações com sistemas externos **instáveis** ou com SLA ruim? Como mitigou?

### Interoperabilidade
- Descreva um cenário em que você integrou **múltiplos sistemas** (internos + externos). Qual foi o maior desafio?
- Como você garante **rastreabilidade** ponta a ponta em fluxos de integração?
- Como você modela fluxos de integração entre sistemas com **formatos de dados diferentes**?

---

## 6. Arquitetura distribuída, microsserviços e resiliência

- Qual sua experiência com **arquitetura de microsserviços** vs. monolito modular?
- Quando você recomendaria microsserviços e quando **não** recomendaria?
- Explique **Clean Architecture** e como você a aplica na prática.
- O que é **DDD (Domain-Driven Design)**? Você já aplicou? Em quê?
- Como você desenha soluções **resilientes** para integrações de alta criticidade?
- O que são os padrões **Saga**, **Outbox** e **Idempotência**? Já implementou algum?
- Como você lida com **consistência eventual** entre serviços?

---

## 7. Mensageria e processamento assíncrono

- Qual sua experiência com **RabbitMQ**? Descreva um fluxo real (produtor, fila, consumidor, DLQ).
- Por que usar **mensageria** em vez de chamadas síncronas HTTP?
- Como você implementa **retentativas** e **dead letter queues**?
- Descreva um caso em que **processamento assíncrono** resolveu um gargalo (ex.: relatórios, OCR, filas de validação).
- Qual a diferença entre **filas** e **pub/sub (exchanges)** no RabbitMQ?
- Como você garante que uma mensagem **não seja processada duas vezes** (idempotência)?

---

## 8. Bancos de dados, cache e armazenamento

### SQL Server e Oracle
- Qual sua experiência com **SQL Server**? Já otimizou consultas complexas? Conte um caso (ex.: redução de 80% no tempo de resposta).
- Qual sua experiência com **Oracle**? Entity Framework Core vs. Dapper vs. PL/SQL — quando usar cada um?
- Como você modela transações em fluxos que envolvem banco + mensageria + API externa?

### Redis e NoSQL
- Como você usa **Redis** em arquiteturas distribuídas (cache, sessão, filas, locks)?
- Quando **não** usar cache? Quais problemas de consistência você já enfrentou?
- Qual sua experiência com bancos **não relacionais**? Em que cenário faria sentido na Softplan?

---

## 9. Testes, qualidade e boas práticas

### Princípios
- Como você aplica os princípios **SOLID** nos seus projetos? Dê um exemplo concreto para cada letra.
- O que é **Clean Code** para você? Quais regras você segue no dia a dia?
- Você pratica **TDD**? Em quais situações faz sentido e em quais não?

### Refatoração
- Como você avalia que uma classe precisa ser **refatorada**? Quais sinais você observa?
- Quais **estratégias de refatoração** você aplica (Extract Method, Strategy, etc.)?
- Conte sobre uma refatoração significativa que você liderou. Qual foi o impacto?

### Testes automatizados
- Qual sua experiência com **testes unitários** e **testes de integração**?
- Como você usa **mocks** e **stubs**? Quando mockar e quando usar dependências reais?
- Como você testa integrações com sistemas externos (sem depender do ambiente real)?
- Já implementou **testes de performance/carga**? Como?

---

## 10. DevOps, containers e CI/CD

- Qual sua experiência com **Docker**? Como você containeriza uma aplicação .NET?
- Descreva um **pipeline de CI/CD** que você configurou ou mantém (build, test, deploy).
- Como funciona o **versionamento com Git** no seu time (branching strategy — GitFlow, trunk-based)?
- Como você faz **deploy** em produção com segurança (blue/green, canary, feature flags)?
- Qual sua experiência com **Azure DevOps** ou ferramentas similares?

---

## 11. Observabilabilidade, troubleshooting e sustentação

- Como você monitora aplicações em produção? Quais ferramentas já usou (Sentry, Datadog, New Relic, Elastic)?
- O que você inclui em **logs estruturados** para facilitar troubleshooting de integrações?
- Descreva um **incidente em produção** que você investigou e resolveu. Qual foi sua abordagem?
- Como você implementa **health checks** e **readiness/liveness probes**?
- O que é **distributed tracing**? Como você correlaciona logs entre serviços?

---

## 12. Segurança em integrações

- Como você implementa **autenticação e autorização** em APIs (JWT, OAuth2, Keycloak)?
- Quais cuidados você toma ao integrar com **sistemas de órgãos públicos** (dados sensíveis, LGPD)?
- Como você protege **credenciais e secrets** em aplicações distribuídas?
- O que você verifica em um **code review** relacionado a segurança?

---

## 13. Ecossistema distribuído (S3, Elastic, Keycloak)

> Itens citados como requisito/diferencial na vaga.

- Qual sua experiência com **S3/MinIO** para armazenamento de objetos?
- Como você usa **Elasticsearch** (busca, logs, analytics)?
- Já trabalhou com **Keycloak** ou outro IdP para autenticação centralizada?
- Se não tiver experiência direta: como você aprenderia rapidamente e quais conceitos já domina que se transferem?

---

## 14. IA no dia a dia de desenvolvimento

- Como você usa **ferramentas de IA** (Cursor, Copilot, Claude, etc.) no desenvolvimento?
- Dê um exemplo em que IA **acelerou** uma entrega sua (código, testes, refatoração, análise).
- Como você **valida e revisa** código gerado por IA antes de commitar?
- Quais riscos você vê em confiar cegamente em outputs de IA?
- Conte sobre o sistema de **OCR com IA** que você evoluiu (SERGET/CS Global). Qual foi o impacto no negócio?

---

## 15. Contexto governamental e alta criticidade

> Diferencial da vaga: integrações com órgãos públicos e tribunais.

- Qual sua experiência com sistemas de **alta criticidade** ou **disponibilidade contínua**?
- Já trabalhou com sistemas do **setor público**, jurídico ou regulatório? (Mesmo indiretamente — mobilidade urbana, aviação, etc.)
- Quais desafios específicos você imagina em integrar com **tribunais e órgãos públicos**?
- Como você garante **auditabilidade** e **rastreabilidade** em fluxos de integração?
- O que muda na engenharia quando o sistema impacta **milhões de cidadãos**?

---

## 16. Liderança técnica e colaboração

- Como você conduz **code reviews**? O que você prioriza ao revisar código de integrações?
- Já **mentorou** desenvolvedores juniores ou plenos? Como?
- Como você propõe **melhorias técnicas** para a arquitetura sem impor?
- Como você participa de **decisões técnicas** com Produto e QA?
- Estaria confortável em **contribuir tecnicamente em entrevistas** de novos membros do time?

---

## 17. Perguntas reportadas no Glassdoor (Softplan)

> Relatos de candidatos a vagas sênior na Softplan. Prepare respostas específicas.

| # | Pergunta | Sua resposta |
|---|----------|--------------|
| 1 | Conte sobre um caso de sucesso no seu último emprego | |
| 2 | TDD, DDD, Clean Architecture — qual sua experiência e como aplica? | |
| 3 | Qual sua experiência com Docker? | |
| 4 | Experiências anteriores, conhecimentos e projetos que destaca | |
| 5 | Qual sua experiência na área? Fale sobre você. Conhece a Softplan? | |
| 6 | Como você aplica SOLID nos seus projetos? | |
| 7 | Como avaliar se uma classe precisa ser refatorada? Quais estratégias? | |

---

## 18. Perguntas para fazer aos entrevistadores

> A Softplan encoraja perguntas. Prepare pelo menos 5.

### Para RH / Recrutadora
- Como é o time de integrações hoje (tamanho, senioridade, distribuição)?
- Qual o desafio técnico mais urgente que essa posição vai enfrentar nos primeiros 90 dias?
- Como funciona a cultura de **Anywhere Office** na prática (rituais, comunicação, fuso horário)?

### Para o Líder Técnico
- Quais **sistemas externos** o time integra hoje (tribunais, órgãos, parceiros)?
- Qual a stack atual de integrações (REST, SOAP, RabbitMQ, tecnologias específicas)?
- Como é o processo de **evolução de contratos** quando um parceiro externo muda a API?
- Qual o nível de **autonomia** esperado para decisões arquiteturais?
- Como o time lida com **incidentes** e sustentação de integrações em produção?
- Há iniciativas de **IA** no produto ou no processo de desenvolvimento?

### Sobre carreira e cultura
- Como a Softplan investe no **desenvolvimento técnico** dos sêniors?
- Quais são os critérios de sucesso para essa posição nos primeiros 6 meses?

---

## 19. Checklist final antes da entrevista

### Histórias STAR prontas (mínimo 5)
- [ ] Caso de sucesso técnico (integração, performance, arquitetura)
- [ ] Desafio complexo superado (legado, prazo, ambiguidade)
- [ ] Conflito ou divergência resolvida com stakeholder
- [ ] Erro/falha e aprendizado
- [ ] Contribuição para o time (mentoria, code review, padronização)

### Aderência vaga × currículo (revise se consegue citar exemplo)
- [ ] .NET 8 / ASP.NET Core em produção
- [ ] APIs REST com contratos, JWT, autorização
- [ ] RabbitMQ, filas, workers, retentativas
- [ ] Redis (cache distribuído)
- [ ] SQL Server (otimização de queries)
- [ ] Oracle + EF Core / Dapper
- [ ] Testes automatizados + CI/CD
- [ ] Migração .NET Framework → .NET 8
- [ ] OCR/IA aplicada a negócio
- [ ] Integrações multi-sistema (GOL, mobilidade urbana)
- [ ] Uso produtivo de IA no desenvolvimento

### Logística (entrevista online)
- [ ] Conexão, câmera e áudio testados
- [ ] Ambiente silencioso e bem iluminado
- [ ] Currículo e este documento acessíveis
- [ ] Playbook da Softplan revisado

---

*Boa sorte! Lembre-se: a entrevista é uma troca — mostre como seus conhecimentos podem contribuir para integrações robustas que impactam o setor público brasileiro.*
