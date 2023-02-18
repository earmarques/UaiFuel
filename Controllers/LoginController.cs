using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using UaiFuel.Models.ExceptionCore;
using UaiFuel.Models.Service;
using UaiFuel.Models.Utils;
using UaiFuel.Models.ViewModel;

namespace UaiFuel.Controllers
{
    public class LoginController : Controller
    {
        LoginService service = null;


        public LoginController()
        {
            service = new LoginService();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new UserViewModel());
        }

        [HttpPost]
        public IActionResult Login(UserViewModel vm)
        {
            if ( ! ModelState.IsValid )
            {
                return View(vm);
            }

            UserViewModel user = service.Read(vm.Login, vm.Senha);
            if (user == null)
            {
                ViewBag.Message = "Usuário e/ou senha inválidos!";
                ViewBag.CssClass = FrontUtil.GetCssClass(AlertType.WARNING);
                return View(vm);
            }
            //HttpContext.Session.SetString("user", JsonSerializer.Serialize<UserViewModel>(user));
            FrontUtil.AddToSession<UserViewModel>(this, "user", user);

            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}


