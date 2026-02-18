# AI Architecture & Models

This document describes the agentic architecture, planned AI modules, and system interactions for shope ease's smart features.

---

## System Overview

```mermaid
graph TD
    A[User (Voice/Text)] --input--> B[Frontend UI (JS/HTML)]
    B --API--> C[.NET Core Backend]
    C --gRPC/API--> D[AI Microservices (Python/Node)]
    D --API-- E[AI Models (OpenAI, ASR, Recommendation)]
    C --DB--> F[Database (SQL Server)]
    C --API Calls--> G[External E-Commerce APIs/Sites]
```

---

## Module Breakdown

- **Voice/Chat UI:** Web Speech API, chat window, transcript to backend
- **.NET Backend:** Endpoint routing, session management, shopping logic
- **AI Gateway:** Receives user intent, engages search/recommend/sentiment
- **External Data/Scrapers:** Product data from third-party or internal crawlers
- **AI Models:** LLM (GPT-4), custom recsys, sentiment via open cloud models

---

## Model Choices (Details)

| AI Task         | Model Type                  | Choice                    |
|-----------------|----------------------------|---------------------------|
| ASR (Speech)    | Automatic Speech Recognition| Whisper, Azure Speech     |
| Multilingual    | NLU (translation, intent)  | OpenAI/Microsoft, Google  |
| Product RecSys  | Embedding retrieval, LLM   | OpenAI finetune, vector db|
| Sentiment       | Speech-text classifier     | OpenAI, Azure Cognitive   |
| Scraping        | Web data extraction        | Python Scrapy, Node Puppeteer |

---

## Security & Privacy

- Voice input secured & not stored
- Data processed via secure endpoints
- Opt-in for additional data driven features

---

## Pipeline Example

1. User: "Find a gaming mouse under $70"
2. Voice recognized (ASR module)
3. Intent handled by LLM, parsed to structured query
4. Product database/search invoked, results returned & ranked
5. Rec agent makes recommendation, UI displays it
6. If confirmed: product is auto-added to cart, checkout proceeds with stored info

---
