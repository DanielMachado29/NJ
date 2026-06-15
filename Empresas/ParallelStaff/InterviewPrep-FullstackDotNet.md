# Interview Prep — Fullstack .NET Developer (LATAM Remote)

> **Objetivo:** Simular uma entrevista em inglês via Meet, com tom natural e técnico — não precisa soar formal demais. Leia em voz alta, pause nas respostas e tente responder antes de ler a sua fala.

---

## Antes da entrevista — dicas rápidas

- **Se travar no inglês:** "Let me think for a second..." ou "That's a good question — so basically..." são frases naturais e compram tempo.
- **Não precisa falar rápido.** Clareza > velocidade.
- **Use exemplos reais** do seu currículo (SERGET, CS Global IT, ATIVA, Legal Process Bot).
- **Remote LATAM:** destaque que você já trabalha remoto há anos e está acostumado com time distribuído.
- **Antes de se apresentar:** pergunte o que motivou a abertura da vaga — a resposta revela se o foco é demanda, legado, produção ou backfill, e você direciona sua fala em cima disso.

---

## Simulated Interview

**Interviewer:** Yareli Chen, Senior Engineering Manager  
**Candidate:** Daniel Machado  
**Format:** Video call (Google Meet) — ~45 minutes

---

### Opening & Small Talk

**Yareli:** Hi Daniel, can you hear me okay?

**Daniel:** Hi Yareli! Yes, I can hear you perfectly. Can you hear me well?

**Yareli:** Loud and clear. Thanks for joining today. How's your day going so far?

**Daniel:** Pretty good, thanks. I'm in Brazil — it's afternoon here. I had some coffee and I'm ready to go. How about you?

**Yareli:** Doing well, thanks. We're based in the US, so it's still morning on my side. Alright, let me give you a quick overview of how we'll run this. I'll start with a few questions about your background, then we'll go into some technical topics around .NET and full-stack work, and at the end you'll have time to ask me questions. Sound good?

**Daniel:** Sounds great. Before I introduce myself — could you tell me why this role is open? Is it team growth, a backfill, or a specific need on the product side?

**Yareli:** Good question. It's mostly team growth — we're expanding the engineering team — but there's a specific need behind it. A lot of our day-to-day is maintaining existing .NET applications, some still on Framework with jQuery on the frontend, plus building and integrating APIs. We also need someone comfortable jumping on production issues with the support team when things get urgent. So it's not just greenfield work — it's a mix of legacy, APIs, and keeping things stable in production.

**Daniel:** That's really helpful — thank you. A lot of that lines up with what I've been doing, so let me walk you through my background with that in mind.

**Yareli:** Perfect. Go ahead.

---

### Tell Me About Yourself

**Daniel:** Sure. I'm a Full Stack Developer with a little over five years of professional experience, mostly focused on C# and the .NET ecosystem. I have a Bachelor's in Computer Science from UFJF here in Brazil.

Most of my career has been in enterprise environments — urban mobility, aviation, and legal tech — where the systems are usually high-volume, have complex business rules, and need to stay reliable in production.

Right now I'm at SERGET as a Full Stack Developer. I've been building and maintaining REST APIs with JWT auth, profile-based authorization, and centralized error handling. A big part of my work there has been modernizing legacy modules — migrating from .NET Framework to .NET 6 — and improving performance with things like Redis caching, RabbitMQ for async messaging, and background workers for heavy routines.

Before that, I spent about seven months at CS Global IT leading backend work for an aviation project for GOL Airlines — .NET 8, Oracle, Entity Framework Core, Dapper, and a lot of integration between internal systems. I also built document processing pipelines using OCR and AI to extract financial data from PDFs.

And outside of my day job, I'm building a legal process update chatbot — .NET 8 backend, Docker, CI/CD — where I use AI-assisted development tools pretty heavily to move faster.

So yeah — full stack, but I'd say my sweet spot is backend and APIs, especially when there's legacy code, integrations, or performance challenges involved.

**Yareli:** Nice. You mentioned legacy modernization a couple of times. That's actually a big part of what we do here. We'll dig into that in a bit.

---

### Why This Role & Remote Work

**Yareli:** What caught your attention about this position?

**Daniel:** A few things, honestly. First, it's a full-stack .NET role, which is exactly the stack I've been working with for years. Second, the job description talks a lot about APIs, maintaining existing applications, and working with production issues — that's basically my day-to-day at SERGET.

I also liked that you mention using AI-powered tools in the workflow. I've been using Cursor, MCP, and agents on personal projects and it's changed how fast I can prototype and ship features — so I'm excited that that's part of the culture here.

And the remote setup for LATAM is a great fit for me. I've been working remotely since 2021 across different companies, so I'm used to async communication, clear documentation, and collaborating with teams in different time zones.

**Yareli:** Good to know. How do you usually handle communication when you're working remotely with product and leadership teams?

**Daniel:** I try to be proactive instead of waiting for someone to chase me. If I'm blocked or if a requirement is unclear, I reach out early — usually a quick message on Slack or Teams, or a short call if it's faster.

For bigger features, I like to break things down: a quick summary of what I understood, what I'm planning to build, and any risks I see. That way product and leadership can correct course before I've spent a week going in the wrong direction.

I also document important decisions — API contracts, database changes, anything that affects production — so the support team and other devs aren't guessing later.

---

### C# & .NET — Framework vs Core

**Yareli:** Let's get into some technical stuff. Can you talk about your experience with C# and ASP.NET — both Framework and Core?

**Daniel:** Yeah, definitely. I started with the classic stack — C#, ASP.NET MVC, .NET Framework — especially at ATIVA, where I built web apps and REST APIs end to end with SQL Server on the back.

At SERGET, a lot of my work has been migrating critical modules from .NET Framework to .NET 6. So I know both worlds — the older patterns like Web.config, IIS hosting, and some of the quirks of Framework — and the modern stack with .NET 6, 7, and 8, minimal APIs or controllers, dependency injection out of the box, and cross-platform deployment.

On the Core side, I use ASP.NET Core for REST APIs, background services, middleware for auth and error handling, and Entity Framework Core or Dapper depending on the scenario. EF is great for most CRUD and domain modeling; Dapper I reach for when I need tight control over complex queries or performance-critical reports — which I did a lot at CS Global IT with Oracle.

**Yareli:** When would you choose .NET Framework over Core today, or is it always Core?

**Daniel:** For greenfield projects, I'd almost always go with .NET 8 or the latest LTS. But in real enterprise life, you don't always get that luxury. If you're maintaining a large Framework codebase and a full migration isn't justified yet, you extend and stabilize what exists — maybe extract new features into a Core service alongside it.

The approach I've used is incremental migration: identify bounded modules, migrate them one at a time, keep the API contracts stable so the frontend doesn't break, and use the strangler fig pattern instead of a big-bang rewrite.

---

### APIs — Building, Consuming, Maintaining

**Yareli:** This role involves developing, consuming, and maintaining internal and third-party APIs. Can you give me an example of an API you built and what you focused on?

**Daniel:** At SERGET, I designed REST APIs with clear contracts — consistent URL patterns, proper HTTP verbs, meaningful status codes, and JSON payloads that match what the Angular frontend expects.

On the security side, we use JWT authentication with profile-based authorization, so each endpoint checks roles or claims before executing business logic. I also implemented centralized exception handling middleware, so the API returns structured error responses instead of leaking stack traces to the client.

For maintainability, I separated concerns — controllers thin, business logic in services, data access in repositories. That makes it easier to test and easier for another dev to jump in without reading the entire codebase.

**Yareli:** And what about consuming third-party APIs? Any challenges there?

**Daniel:** Yeah, a few. At ATIVA I built integrations with external services for legal workflows — web crawlers and automation with Selenium and Playwright, but also standard REST integrations.

The usual challenges are authentication — OAuth, API keys, token refresh — inconsistent response formats, and rate limiting. What I typically do is wrap the third-party client in an internal service with retry logic, timeout configuration, and logging. If the external API goes down, our app degrades gracefully instead of crashing.

I also version internal contracts when the third party changes their API, so we can adapt without breaking our consumers.

**Yareli:** REST, JSON, XML — how comfortable are you with all of that?

**Daniel:** REST and JSON are my daily drivers — that's what I use on every project. XML I haven't used as much recently, but I've worked with it in older enterprise integrations and I'm comfortable parsing and producing it when needed. The concepts transfer — it's still about contracts, serialization, and validation.

---

### SQL & Relational Databases

**Yareli:** Tell me about your experience with SQL and relational databases.

**Daniel:** I've worked mostly with SQL Server, but also Oracle and PostgreSQL. At SERGET it's SQL Server — modeling tables, stored procedures when they make sense, and a lot of query tuning for reports and operational screens. I optimized several queries that were slowing down dashboards, and the response times dropped significantly.

At CS Global IT with GOL Airlines, it was Oracle — PL/SQL, EF Core, and Dapper side by side. Complex joins, financial data, traceability requirements.

I'm comfortable with normalization, indexes, execution plans, and knowing when *not* to push everything to the database. I also think about migrations — how schema changes roll out without downtime when possible.

**Yareli:** How do you approach a slow query in production?

**Daniel:** First I'd look at the actual execution plan and see if we're doing table scans or missing indexes. Then check if the ORM is generating N+1 queries — that's a classic one with EF Core.

I'd reproduce it in staging with realistic data volume, because a query that flies in dev can die in production. Fixes might be indexes, rewriting the query, caching with Redis if the data doesn't need to be real-time, or moving heavy reporting to a read replica or a pre-aggregated table.

And I'd add logging or APM metrics so we catch the next one before users complain.

---

### Frontend — JavaScript, HTML, CSS, jQuery

**Yareli:** On the frontend side, we still have some legacy pieces using jQuery and AJAX. What's your experience there?

**Daniel:** My recent frontend work has been mostly Angular and TypeScript — that's what I use at SERGET and used at ATIVA. But underneath, it's still HTML, CSS, and JavaScript, and I'm comfortable going into the fundamentals when needed.

I haven't used jQuery heavily in the last couple of years, but I know the patterns — DOM manipulation, event handlers, `$.ajax` calls to backend endpoints. It's straightforward, and a lot of legacy apps still rely on it. I'm totally fine maintaining and enhancing that kind of code while we gradually modernize.

For AJAX and API consumption from the frontend, I've done it both with jQuery-style calls and with Angular's HttpClient — same idea: handle errors, loading states, and don't trust the client for security.

**Yareli:** So if you had to fix a bug in a jQuery-heavy page that calls a .NET API, you'd be comfortable?

**Daniel:** Yeah, absolutely. I'd start by tracing the network call in DevTools, check the request payload and response, then follow it to the API endpoint on the backend. Usually the bug is either a contract mismatch, a validation issue, or something in the DOM logic. I'm used to working across both sides.

---

### Legacy Systems & Production Support

**Yareli:** Comfort with legacy systems is something we really need. Can you share a specific example?

**Daniel:** At SERGET we have urban mobility and radar management systems that started on .NET Framework. Some modules were fragile — tight coupling, little test coverage, and performance bottlenecks under high traffic.

I contributed to migrating key modules to .NET 6 with a cleaner architecture — separating domain logic, introducing async processing with RabbitMQ for heavy workflows, and Redis for caching frequently accessed data.

The trickiest part wasn't the code itself — it was doing it without breaking production. We used feature flags, parallel runs where possible, and lots of monitoring during cutover. I also partnered with the support team when users reported edge cases we hadn't seen in staging.

**Yareli:** Tell me about a production issue you helped troubleshoot.

**Daniel:** We had a routine that processed a high volume of infractions, and it started timing out during peak hours. Users on the operational screens noticed delays.

I traced it to synchronous processing that blocked the main flow. We moved that work to background services with a queue, added retry logic for failures, and cached reference data in Redis that was being hit on every request.

I worked with support to understand which screens were affected and communicated timelines. After the fix, stability improved a lot — no more bottlenecks during peak load.

**Yareli:** How do you balance fixing production fires vs. planned feature work?

**Daniel:** Production issues come first if users are impacted — but I try to fix the root cause, not just patch symptoms. If we're getting repeated tickets about the same area, that's a signal we need refactoring or better monitoring.

For priorities, I stay in sync with my lead or product — quick message like "this bug is affecting X users, I'll switch to it, feature Y slips by a day." Transparency helps everyone.

---

### Performance, Reliability & Scalability

**Yareli:** How do you think about performance and scalability when you're building a feature?

**Daniel:** I start with the expected load — how many users, how much data, is it read-heavy or write-heavy. Then I design the simplest thing that works, but with clear extension points.

On the API side: pagination, filtering on the database not in memory, async for long operations, caching where data is stale-tolerant. On the infrastructure side: horizontal scaling works better when the app is stateless — JWT helps with that.

At SERGET, RabbitMQ and background workers were key for scalability — we decoupled heavy processing from the HTTP request so the API stays responsive.

I also think about observability from day one — structured logs, health checks, metrics — because you can't scale what you can't measure.

---

### Agile, Git & Development Workflow

**Yareli:** What's your experience with agile and source control?

**Daniel:** I've worked in Scrum environments — sprints, daily standups, refinements, retros. At ATIVA that was the main workflow. I like Kanban too for maintenance-heavy work.

For Git, I use feature branches, pull requests, and code reviews every day. At CS Global IT we used Azure DevOps for pipelines and repos; I've also used GitHub and GitLab. I'm comfortable with merge conflicts, rebasing when the team prefers it, and writing commits that actually explain *why* something changed.

**Yareli:** What does a good pull request look like to you?

**Daniel:** Small and focused — one logical change. Clear description, link to the ticket, screenshots or API examples if it's a UI or contract change. Tests when it makes sense. And I review my own diff before asking someone else to — catches a lot of silly mistakes.

---

### Automated Testing

**Yareli:** We listed automated tests as a nice-to-have. What's your experience there?

**Daniel:** I use xUnit and NUnit with Moq for unit tests — mostly testing business logic, services, and edge cases. I'm not going to pretend every project I've been on had 90% coverage, but I push for tests on critical paths: auth, payment-like flows, anything that would hurt production if it broke.

I've done integration tests for APIs too — hitting real endpoints against a test database. And I follow TDD when the logic is complex enough that it pays off.

For me, automated tests are part of maintainability — especially on legacy code. Before refactoring, I try to add a safety net of tests around the behavior we're keeping.

---

### AI-Assisted Development

**Yareli:** We encourage using AI tools for development, testing, and research. How do you use them today?

**Daniel:** Pretty actively, especially on my Legal Process Update Bot project. I use Cursor with MCP and agents to scaffold features, explore unfamiliar APIs, and speed up boilerplate — things that used to take a week now take a couple of days.

But I don't copy-paste blindly. I always review generated code, run tests, and make sure it fits our architecture and security standards. AI is great for drafts, refactors, and "how does this library work?" research — not a replacement for understanding what you're shipping.

At SERGET I also worked on OCR-based AI inspection for traffic infractions — so I'm comfortable integrating AI into real business workflows, not just using Copilot for autocomplete.

**Yareli:** Any concerns or pitfalls you've noticed with AI-assisted coding?

**Daniel:** Yeah — it can hallucinate APIs that don't exist, or suggest patterns that look clean but don't fit your codebase. It can also miss security details like SQL injection or improper auth checks. So I treat it like a fast junior dev: great output, needs review.

---

### Nice-to-Have Topics

**Yareli:** Quick ones — Blazor, CMS platforms, SEO, mobile. Any exposure?

**Daniel:** Blazor I've explored — I haven't shipped a production Blazor app yet, but I understand the component model and I'd pick it up quickly since it's still .NET. My frontend production experience is mainly Angular.

CMS — I haven't worked deeply with Umbraco, Drupal, or WordPress in enterprise projects, but I'm familiar with the concept and content-driven architectures.

SEO — basic best practices like semantic HTML, meta tags, performance impacting rankings — I'm aware, though my roles have been more backend and internal apps.

Mobile — my focus has been web and APIs, not native iOS/Android. But REST APIs I build are usually consumed by mobile clients, so I think about mobile-friendly payloads and versioning.

**Yareli:** Honest answer — I appreciate that.

---

### Behavioral & Soft Skills

**Yareli:** Tell me about a time you disagreed with a technical decision. How did you handle it?

**Daniel:** On a migration project, there was a push to rewrite a large module in one shot. I was concerned about risk and downtime.

I didn't just say "that's wrong." I put together a short comparison — big-bang vs. incremental migration — with effort estimates and risk points. We ended up agreeing on a phased approach, migrating one bounded context at a time.

I think it's fine to disagree, but you need data and respect. Once the team decides, I commit to the decision even if it wasn't my first choice.

**Yareli:** How do you handle feedback on your code during reviews?

**Daniel:** I try to detach ego from code — if someone spots a bug or a cleaner pattern, that's a win. I ask questions when I don't understand the feedback, and I look for recurring themes — if three people say my PRs are too big, that's on me to change.

---

### Your Questions for the Interviewer

**Yareli:** Alright Daniel, we're coming up on time. What questions do you have for me?

**Daniel:** A few, if that's okay.

First — what does the current stack look like day to day? Is it mostly .NET Core now, or is there still a significant .NET Framework footprint?

**Yareli:** *(answers — listen and nod, take notes)*

**Daniel:** Got it. And how is the team structured — would I be embedded with a product squad, or more of a shared platform team across apps?

**Yareli:** *(answers)*

**Daniel:** That makes sense. One more — you mentioned AI tools in the workflow. Is the team already standardized on something like Copilot or Cursor, or is it still figure-it-out-yourself?

**Yareli:** *(answers)*

**Daniel:** Cool. Last one — what would success look like for someone in this role in the first six months?

**Yareli:** *(answers)*

**Daniel:** That's really helpful. Those were my main questions.

---

### Closing

**Yareli:** Great questions. Thanks for your time today, Daniel. We'll be in touch with next steps within the next few days.

**Daniel:** Thank you, Yareli. I really enjoyed the conversation — the role sounds like a strong match with what I've been doing, especially the legacy modernization and API work. Have a great rest of your day!

**Yareli:** You too. Bye!

**Daniel:** Bye!

---

## Perguntas extras para praticar sozinho

Responda em voz alta em inglês. Use exemplos do seu currículo.

1. **What's the difference between `IEnumerable`, `IQueryable`, and when would you use each?**
2. **How does dependency injection work in ASP.NET Core?**
3. **Explain JWT authentication flow in your own words.**
4. **What's the difference between `async`/`await` and background services or queues?**
5. **How do you handle database migrations in a team environment?**
6. **Describe a time you had to learn a new technology quickly for a project.**
7. **What would you do in your first 30 days in this role?**

---

## Vocabulário útil para a entrevista

| Termo | Quando usar |
|-------|-------------|
| *Let me walk you through...* | Começar uma explicação |
| *The main challenge was...* | Falar de dificuldades |
| *What I typically do is...* | Descrever seu processo |
| *Does that make sense?* | Checar se a entrevistadora acompanhou |
| *I don't have production experience with X, but...* | Responder honestamente um gap |
| *Happy to dive deeper if you want* | Oferecer mais detalhe |

---

## Gaps honestos — como responder se perguntarem

| Tópico da vaga | Sua situação | Frase sugerida |
|----------------|--------------|----------------|
| jQuery | Pouco uso recente | *"Most of my recent frontend is Angular/TypeScript, but I'm comfortable with jQuery and AJAX for legacy pages — I've debugged plenty of API + frontend issues across both."* |
| Blazor | Explorou, não produção | *"I've explored Blazor but haven't shipped it to production yet. Given my .NET and component-based experience with Angular, I'm confident I'd ramp up quickly."* |
| CMS (Umbraco, etc.) | Sem experiência enterprise | *"I haven't worked with Umbraco or Drupal in enterprise projects, but I understand CMS patterns and I'm used to integrating content-driven requirements via APIs."* |
| XML | Menos recente | *"JSON and REST are my daily work; XML I'm comfortable with from older integrations when the contract requires it."* |

---

## Checklist no dia da entrevista

- [ ] Testar câmera, microfone e internet 15 min antes
- [ ] Ambiente quieto, boa luz no rosto
- [ ] Água por perto
- [ ] Currículo e Job Description abertos (só para consulta, não ler)
- [ ] Anotar 3 perguntas para fazer no final
- [ ] Respirar — pausas curtas são normais e naturais em inglês

---

*Boa sorte, Daniel. Seu perfil encaixa bem na vaga: .NET full stack, APIs, legado, SQL, remoto, AI tools e experiência enterprise. A entrevista é uma conversa — não um exame de gramática.*
