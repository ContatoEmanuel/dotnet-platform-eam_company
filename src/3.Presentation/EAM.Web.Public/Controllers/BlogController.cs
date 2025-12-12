using Microsoft.AspNetCore.Mvc;

namespace EAM.Web.Public.Controllers;

public class BlogController : Controller
{
    public IActionResult Index()
    {
        // TODO: Integrar com serviço de blog no futuro
        var posts = new[]
        {
            new {
                Id = 1,
                Title = "Começando com Dynamics 365 Customer Engagement",
                Excerpt = "Aprenda os conceitos fundamentais do Dynamics 365 CE e como começar seu primeiro projeto.",
                Date = new DateTime(2024, 12, 10),
                Author = "Emanuel Macêdo",
                Category = "Dynamics 365",
                ReadTime = "5 min",
                ImageUrl = "https://via.placeholder.com/400x250/3B82F6/FFFFFF?text=Dynamics+365"
            },
            new {
                Id = 2,
                Title = "Power Platform: Automatizando processos com Power Automate",
                Excerpt = "Descubra como criar automações poderosas usando Power Automate e integrar com diversas aplicações.",
                Date = new DateTime(2024, 12, 8),
                Author = "Emanuel Macêdo",
                Category = "Power Platform",
                ReadTime = "7 min",
                ImageUrl = "https://via.placeholder.com/400x250/6366F1/FFFFFF?text=Power+Automate"
            },
            new {
                Id = 3,
                Title = "Arquitetura Clean em .NET: Princípios e Práticas",
                Excerpt = "Entenda os conceitos de Clean Architecture e como aplicar em projetos .NET Core.",
                Date = new DateTime(2024, 12, 5),
                Author = "Emanuel Macêdo",
                Category = ".NET",
                ReadTime = "10 min",
                ImageUrl = "https://via.placeholder.com/400x250/8B5CF6/FFFFFF?text=Clean+Architecture"
            }
        };

        ViewBag.Posts = posts;
        return View();
    }

    public IActionResult Post(int id)
    {
        // TODO: Buscar post específico do serviço
        var post = new {
            Id = id,
            Title = "Começando com Dynamics 365 Customer Engagement",
            Content = @"
                <p class='lead'>O Dynamics 365 Customer Engagement é uma plataforma poderosa para gerenciar relacionamentos com clientes.</p>
                
                <h2>O que é Dynamics 365 CE?</h2>
                <p>Dynamics 365 Customer Engagement (CE) é uma solução CRM (Customer Relationship Management) que ajuda organizações a gerenciar vendas, marketing, atendimento ao cliente e operações de campo.</p>
                
                <h2>Principais Componentes</h2>
                <ul>
                    <li><strong>Sales:</strong> Gerenciamento de oportunidades e pipeline de vendas</li>
                    <li><strong>Marketing:</strong> Automação de marketing e gestão de campanhas</li>
                    <li><strong>Customer Service:</strong> Atendimento ao cliente e gestão de casos</li>
                    <li><strong>Field Service:</strong> Operações de campo e agendamento</li>
                </ul>
                
                <h2>Por onde começar?</h2>
                <p>Para iniciar com Dynamics 365, recomendo seguir estes passos:</p>
                <ol>
                    <li>Criar uma conta de trial no Microsoft 365</li>
                    <li>Acessar o Power Platform Admin Center</li>
                    <li>Provisionar um ambiente Dynamics 365</li>
                    <li>Explorar os aplicativos model-driven disponíveis</li>
                </ol>
                
                <p>Nos próximos artigos, vamos aprofundar em cada um desses componentes e criar soluções práticas.</p>
            ",
            Date = new DateTime(2024, 12, 10),
            Author = "Emanuel Macêdo",
            Category = "Dynamics 365",
            ReadTime = "5 min",
            ImageUrl = "https://via.placeholder.com/1200x400/3B82F6/FFFFFF?text=Dynamics+365"
        };

        ViewBag.Post = post;
        return View();
    }
}
