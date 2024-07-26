using HeThongNganHang.Models;
using HeThongNganHang.Services;
using Microsoft.AspNetCore.Mvc;

namespace HeThongNganHang.Controllers;
[Route("login")]
public class LoginController : Controller
{
    private AccountService accountService;
    public LoginController(AccountService _accountService)
    {
        accountService = _accountService;
    }

    [Route("~/")]
    [Route("login")]
    public IActionResult login()
    {
        return View("login");
    }

    [HttpPost]
    [Route("process")]
    public IActionResult Process(string username, string password)
    {
        if (accountService.Login(username, password))
        {
            HttpContext.Session.SetString("username", username);
            return RedirectToAction("index", "home");
        }
        else
        {
            TempData["Msg"] = "Wrong username or password";
            return RedirectToAction("login", "login");
        }
    }

    [Route("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("username");
        return RedirectToAction("login", "login");
    }

    [HttpGet]
    [Route("signup")]
    public IActionResult register()
    {
        return View("register");
    }

    [HttpPost]
    [Route("signup")]
    public IActionResult register(Account account)
    {
        account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
        if (accountService.Create(account))
        {
            TempData["Msg"] = "Signup Sucess";
            return RedirectToAction("login", "login");
        }
        else
        {
            TempData["Msg"] = "Signup Failed";
            return RedirectToAction("register", "login");
        }
    }

}
