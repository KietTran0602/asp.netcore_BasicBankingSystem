using HeThongNganHang.Models;
using HeThongNganHang.Services;
using Microsoft.AspNetCore.Mvc;

namespace HeThongNganHang.Controllers;
[Route("profile")]
public class ProfileController : Controller
{
    private AccountService accountService;
    private TransactionService transactionService;

    public ProfileController(AccountService _accountService, TransactionService _transactionService)
    {
        accountService = _accountService;
        transactionService = _transactionService;
    }
    [HttpGet]
    [Route("edit")]
    public IActionResult Edit()
    {
        var username = HttpContext.Session.GetString("username");
        var account = accountService.findByUsername(username);
        ViewBag.Account = account;
        return View("edit", account);
    }

    [HttpPost]
    [Route("edit")]
    public IActionResult Edit(Account account)
    {
        var username = HttpContext.Session.GetString("username");
        var currentaccount = accountService.findByUsername(username);

        if (!string.IsNullOrEmpty(account.Password))
        {
            currentaccount.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
        }
        currentaccount.FullName = account.FullName;
        currentaccount.Email = account.Email;
        currentaccount.Phone = account.Phone;
        if (accountService.Update(currentaccount))
        {
            TempData["uMsg"] = "Cập nhật thông tin cá nhân thành công";
            return RedirectToAction("Index", "home");
        }
        else
        {
            TempData["uMsg"] = "Cập nhật thông tin cá nhân thất bại";
            return RedirectToAction("edit", "profile");
        }
    }
}
