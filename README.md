# GPrev - Análise CNIS

Sistema para análise de extratos previdenciários do CNIS (Cadastro Nacional de Informações Sociais).
Contempla leitura de remunerações com cálculo de contribuições e excedentes.

## Como Usar

### 1. Executar a Aplicação
```bash
dotnet run --project src/GPrev.Forms
```

## Desenvolvimento

### Executar Testes
```bash
dotnet test
```

### Compilar Solução
```bash
dotnet build
```

### Estrutura do Projeto
```
GPrev/
├── src/
│   ├── GPrev.Core/             # Biblioteca principal
│   │   ├── Dominio/                # Regras de negócio
│   │   │   ├── Models/                 # Modelos de dados de negócios
│   │   │   └── Services/               # Services de negócios
│   │   ├── DI/                     # Injeção de dependências
│   │   ├── DTO/                    # Objetos para retorno
│   │   └── Infra/                  # Infraestrutura, manuseio de arquivos
│   └── GPrev.Forms/            # Interface Windows Forms
└── tests/
```

## Tecnologias

- **.NET 9.0**: Framework principal
- **Windows Forms**: Interface desktop
- **PdfPig**: Processamento de arquivos PDF
- **Microsoft.Extensions.DependencyInjection**: Injeção de dependência

## Requisitos

- .NET 9.0 Runtime/SDK
- Windows (para Windows Forms)
- Arquivos PDF do CNIS para processamento

## Licença

Distribuído sob a [MIT License](LICENSE).  
Consulte [THIRD_PARTY_NOTICES.md](THIRD_PARTY_NOTICES.md) para avisos de licenças de terceiros.