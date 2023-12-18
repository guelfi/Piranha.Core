using Microsoft.AspNetCore.Mvc;
using Piranha;
using Piranha.Extend;
using Piranha.Extend.Fields;
using Piranha.Extend.Blocks;
using Piranha.Models;
using RazorWeb.Models;

namespace RazorWeb.Controllers;

/// <summary>
/// This controller is only used when the project is first started
/// and no pages has been added to the database. Feel free to remove it.
/// </summary>
[ApiExplorerSettings(IgnoreApi = true)]
public class SetupController : Controller
{
    private readonly IApi _api;

    public SetupController(IApi api)
    {
        _api = api;
    }

    [Route("/")]
    public IActionResult Index()
    {
        return View();
    }

    [Route("/seed")]
    public async Task<IActionResult> Seed()
    {
        var images = new Dictionary<string, Guid>();

        // Get the default site
        var site = await _api.Sites.GetDefaultAsync();

        // Add media assets
        foreach (var image in Directory.GetFiles("seed"))
        {
            var info = new FileInfo(image);
            var id = Guid.NewGuid();
            images.Add(info.Name, id);

            using (var stream = System.IO.File.OpenRead(image))
            {
                await _api.Media.SaveAsync(new Piranha.Models.StreamMediaContent()
                {
                    Id = id,
                    Filename = info.Name,
                    Data = stream
                });
            }
        }

        // Adicionar página do blog
        var blogPage = await StandardArchive.CreateAsync(_api);
        blogPage.Id = Guid.NewGuid();
        blogPage.SiteId = site.Id;
        blogPage.Title = "Arquivo do Blog";
        blogPage.NavigationTitle = "Blog";
        blogPage.MetaKeywords = "Purus, Amet, Ullamcorper, Fusce";
        blogPage.MetaDescription = "Integer posuere erat a ante venenatis dapibus posuere velit aliquet.";
        blogPage.PrimaryImage = images["woman-in-blue-long-sleeve-dress-standing-beside-brown-wooden-4100766.jpg"];
        blogPage.Excerpt = "Mantenha-se atualizado com as últimas e melhores notícias. Todo esse conhecimento está ao seu alcance, o que você está esperando?";
        blogPage.Published = DateTime.Now;

        await _api.Pages.SaveAsync(blogPage);

        // Adicionar página inicial
        var startPage = await StandardPage.CreateAsync(_api);
        startPage.Id = Guid.NewGuid();
        startPage.SiteId = site.Id;
        startPage.Title = "Bem-vindo ao Gerencador de Conteudo";
        startPage.NavigationTitle = "Início";
        startPage.MetaTitle = "CMS de Código Aberto, Multiplataforma ASP.NET Core";
        startPage.MetaKeywords = "Purus, Amet, Ullamcorper, Fusce";
        startPage.MetaDescription = "Integer posuere erat a ante venenatis dapibus posuere velit aliquet.";
        startPage.PrimaryImage = images["cute-business-kids-working-on-project-together-surfing-3874121.jpg"];
        startPage.Excerpt = "Bem-vindo ao seu novo site. Para mostrar alguns dos recursos que você tem disponíveis, criamos algum conteúdo de exemplo para você.";
        startPage.Published = DateTime.Now;

        startPage.Blocks.Add(new HtmlBlock
        {
            Body =
                "<h2>Porque as Primeiras Impressões Duram</h2>" +
                "<p class=\"lead\">Todas as páginas e postagens que você cria têm uma imagem primária e um resumo disponível que você pode usar para criar cabeçalhos atraentes para o seu conteúdo, mas também ao listar ou vincular a ele em seu site. Esses campos são totalmente opcionais e podem ser desativados para cada tipo de conteúdo.</p>"
        });
        startPage.Blocks.Add(new ColumnBlock
        {
            Items = new List<Block>()
        {
            new ImageBlock
            {
                Aspect = new SelectField<ImageAspect>
                {
                    Value = ImageAspect.Widescreen
                },
                Body = images["concentrated-little-kids-taking-notes-in-organizer-and-3874109.jpg"]
            },
            new HtmlBlock
            {
                Body =
                    "<h3>Adicione, Edite & Reorganize</h3>" +
                    "<p class=\"lead\">Construa seu conteúdo com nosso poderoso e modular editor de blocos que permite adicionar, reorganizar e dispor seu conteúdo com facilidade.</p>" +
                    "<p>Novos blocos de conteúdo podem ser instalados ou criados em seu projeto e estarão disponíveis para uso em todas as funções de conteúdo. Construa regiões complexas para todo o conteúdo fixo que você deseja em seus tipos de conteúdo.</p>"
            }
        }
        });
        startPage.Blocks.Add(new HtmlBlock
        {
            Body =
                "<h3>Cross-Link Your Content</h3>" +
                "<p>Com nossos novos blocos de link de página e postagem, é mais fácil do que nunca promover e vincular seu conteúdo em todo o site. Basta selecionar o conteúdo que você deseja referenciar e simplesmente usar seus campos básicos, incluindo imagem primária e resumo para exibi-lo.</p>"
        });
        startPage.Blocks.Add(new ColumnBlock
        {
            Items = new List<Block>
        {
            new PageBlock
            {
                Body = blogPage
            }
        }
        });
        startPage.Blocks.Add(new HtmlBlock
        {
            Body =
                "<h2>Compartilhe Suas Imagens</h2>" +
                "<p>Uma imagem diz mais que mil palavras. Com nossa <strong>Galeria de Imagens</strong>, você pode facilmente criar uma galeria ou carrossel e compartilhar qualquer coisa que você tenha disponível em sua biblioteca de mídia ou baixar novas imagens diretamente em sua página apenas arrastando-as para o seu navegador.</p>"
        });
        startPage.Blocks.Add(new ImageGalleryBlock
        {
            Items = new List<Block>
        {
            new ImageBlock
            {
                Body = images["cheerful-diverse-colleagues-working-on-laptops-in-workspace-3860809.jpg"]
            },
            new ImageBlock
            {
                Body = images["smiling-woman-working-in-office-with-coworkers-3860641.jpg"]
            },
            new ImageBlock
            {
                Body = images["diverse-group-of-colleagues-having-meditation-together-3860619.jpg"]
            }
        }
        });

        await _api.Pages.SaveAsync(startPage);

        // Adicionar postagens de blog
        var post1 = await StandardPost.CreateAsync(_api);
        post1.BlogId = blogPage.Id;
        post1.Category = "Magna";
        post1.Tags.Add("Euismod", "Ridiculus");
        post1.Title = "Tortor Magna Ultricies";
        post1.MetaKeywords = "Nibh, Vulputate, Venenatis, Ridiculus";
        post1.MetaDescription = "Integer posuere erat a ante venenatis dapibus posuere velit aliquet. Maecenas faucibus mollis interdum.";
        post1.PrimaryImage = images["smiling-woman-working-in-office-with-coworkers-3860641.jpg"];
        post1.Excerpt = "Maecenas faucibus mollis interdum. Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec sed odio dui.";
        post1.Published = DateTime.Now;

        post1.Blocks.Add(new HtmlBlock
        {
            Body =
                "<p>O comando Praesent cursus magna, vel scelerisque nisl consectetur et. Vestibulum id ligula porta felis euismod semper. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nullam quis risus eget urna mollis ornare vel eu leo. Nullam id dolor id nibh ultricies vehicula ut id elit. Nullam quis risus eget urna mollis ornare vel eu leo. Aenean eu leo quam. Pellentesque ornare sem lacinia quam venenatis vestibulum.</p>" +
                "<p>Aenean eu leo quam. Pellentesque ornare sem lacinia quam venenatis vestibulum. Praesent commodo cursus magna, vel scelerisque nisl consectetur et. Maecenas sed diam eget risus varius blandit sit amet non magna. Nullam id dolor id nibh ultricies vehicula ut id elit. Maecenas faucibus mollis interdum. Cras mattis consectetur purus sit amet fermentum. Donec ullamcorper nulla non metus auctor fringilla.</p>" +
                "<p>Sed posuere consectetur est at lobortis. Maecenas faucibus mollis interdum. Sed posuere consectetur est at lobortis. Morbi leo risus, porta ac consectetur ac, vestibulum at eros. Nullam id dolor id nibh ultricies vehicula ut id elit. Maecenas faucibus mollis interdum.</p>"
        });
        await _api.Posts.SaveAsync(post1);

        var post2 = await StandardPost.CreateAsync(_api);
        post2.BlogId = blogPage.Id;
        post2.Category = "Tristique";
        post2.Tags.Add("Euismod", "Ridiculus");
        post2.Title = "Sollicitudin Risus Dapibus";
        post2.MetaKeywords = "Nibh, Vulputate, Venenatis, Ridiculus";
        post2.MetaDescription = "Integer posuere erat a ante venenatis dapibus posuere velit aliquet. Maecenas faucibus mollis interdum.";
        post2.PrimaryImage = images["concentrated-little-kids-taking-notes-in-organizer-and-3874109.jpg"];
        post2.Excerpt = "Donec sed odio dui. Maecenas faucibus mollis interdum. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.";
        post2.Published = DateTime.Now;

        post2.Blocks.Add(new HtmlBlock
        {
            Body =
                "<p>Praesent commodo cursus magna, vel scelerisque nisl consectetur et. Vestibulum id ligula porta felis euismod semper. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nullam quis risus eget urna mollis ornare vel eu leo. Nullam id dolor id nibh ultricies vehicula ut id elit. Nullam quis risus eget urna mollis ornare vel eu leo. Aenean eu leo quam. Pellentesque ornare sem lacinia quam venenatis vestibulum.</p>" +
                "<p>Aenean eu leo quam. Pellentesque ornare sem lacinia quam venenatis vestibulum. Praesent commodo cursus magna, vel scelerisque nisl consectetur et. Maecenas sed diam eget risus varius blandit sit amet non magna. Nullam id dolor id nibh ultricies vehicula ut id elit. Maecenas faucibus mollis interdum. Cras mattis consectetur purus sit amet fermentum. Donec ullamcorper nulla non metus auctor fringilla.</p>" +
                "<p>Sed posuere consectetur est at lobortis. Maecenas faucibus mollis interdum. Sed posuere consectetur est at lobortis. Morbi leo risus, porta ac consectetur ac, vestibulum at eros. Nullam id dolor id nibh ultricies vehicula ut id elit. Maecenas faucibus mollis interdum.</p>"
        });
        await _api.Posts.SaveAsync(post2);

        var post3 = await StandardPost.CreateAsync(_api);
        post3.Id = Guid.NewGuid();
        post3.BlogId = blogPage.Id;
        post3.Category = "Piranha";
        post3.Tags.Add("Development", "Release Info");
        post3.Title = "What's New In 10.0";
        post3.Slug = "whats-new";
        post3.MetaKeywords = "Piranha, Version, Information";
        post3.MetaDescription = "Here you can find information about what's included in the current release.";
        post3.PrimaryImage = images["bird-s-eye-view-photography-of-lighted-city-3573383.jpg"];
        post3.Excerpt = "Here you can find information about what's included in the current release.";
        post3.Published = DateTime.Now;

        post3.Blocks.Add(new HtmlBlock
        {
            Body =
                "<p class=\"lead\">Big thanks to <a href=\"https://github.com/aatmmr\">@aatmmr</a>, <a href=\"https://github.com/brianpopow\">@brianpopow</a> and <a href=\"https://github.com/tedvanderveen\">@tedvanderveen</a> for their contributions and all of the people who has helped with translating the manager.</p>"
        });
        post3.Blocks.Add(new ColumnBlock
        {
            Items = new List<Block>
            {
                new MarkdownBlock
                {
                    Body =
                        "#### Core\n\n" +
                        "* Remove the need to use MARS for SQL Server [#1417](https://github.com/piranhacms/piranha.core/issues/1417)\n" +
                        "* Detect EXIF orientation on mobile pictures [#1442](https://github.com/piranhacms/piranha.core/issues/1442)\n" +
                        "* Update BlobStorage to use Azure.Storage.Blobs [#1564](https://github.com/piranhacms/piranha.core/pull/1564)\n" +
                        "* Update Pomelo.EntityFrameworkCore.MySql [#1646](https://github.com/piranhacms/piranha.core/pull/1646)\n" +
                        "* Add sort order to fields [#1732](https://github.com/piranhacms/piranha.core/issues/1732)\n" +
                        "* Update to .NET 6 [#1733](https://github.com/piranhacms/piranha.core/issues/1733)\n" +
                        "* Use Identify to get image width and height [#1734](https://github.com/piranhacms/piranha.core/pull/1734)\n" +
                        "* Clean up application startup [#1738](https://github.com/piranhacms/piranha.core/issues/1738)\n" +
                        "* Add markdown block [#1744](https://github.com/piranhacms/piranha.core/issues/1744)\n" +
                        "* Remove description attributes [#1747](https://github.com/piranhacms/piranha.core/issues/1747)\n\n" +
                        "#### Manager\n\n" +
                        "* Add content settings (with region support) [#1524](https://github.com/piranhacms/piranha.core/issues/1524)\n" +
                        "* Update Summernote package [#1730](https://github.com/piranhacms/piranha.core/issues/1730)\n" +
                        "* Manager security update [#1741](https://github.com/piranhacms/piranha.core/issues/1741)\n" +
                        "* Redesign Add page button in manager [#1748](https://github.com/piranhacms/piranha.core/issues/1748)\n\n" +
                        "#### Bugfixes\n\n" +
                        "* Cannot access disposed object [#1701](https://github.com/piranhacms/piranha.core/issues/1701)\n" +
                        "* Invalid PageField URL in Manager [#1705](https://github.com/piranhacms/piranha.core/issues/1705)\n"
                },
                new ImageBlock
                {
                    Body = images["person-looking-at-phone-and-at-macbook-pro-1181244.jpg"]
                }
            }
        });

        await _api.Posts.SaveAsync(post3);

        var comment = new Piranha.Models.PostComment
        {
            Author = "Håkan Edling",
            Email = "hakan@tidyui.com",
            Url = "http://piranhacms.org",
            Body = "Awesome to see that the project is up and running! Now maybe it's time to start customizing it to your needs. You can find a lot of information in the official docs.",
            IsApproved = true
        };
        await _api.Posts.SaveCommentAsync(post3.Id, comment);

        return Redirect("~/");
    }
}
