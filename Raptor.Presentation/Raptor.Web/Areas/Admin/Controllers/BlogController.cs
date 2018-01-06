using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Raptor.Services.Blog;
using Raptor.Web.Areas.Admin.ViewModels;
using System.Linq;

namespace Raptor.Web.Areas.Admin.Controllers
{
    [Authorize]
    [Area("admin")]
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService) {
            _blogService = blogService;
        }

        public IActionResult Index() {
            return View();
        }

        [HttpGet]
        [Route("admin/blog/posts/create")]
        public IActionResult Create() {
            var model = new BlogPostViewModel() {
                PageTitle = "Create Blog Post",
                Action = "create",
                BlogPostCategories = new SelectList(_blogService.GetBlogPostCategories().ToList(), "PostCategoryId", "Name")
            };

            return View("BlogPostView", model);
        }
    }
}