using HeThongNganHang.Helpers;
using HeThongNganHang.Models;
using HeThongNganHang.Services;
using Microsoft.AspNetCore.Mvc;

namespace HeThongNganHang.Controllers;
[Route("home")]
public class HomeController : Controller
{
    private AccountService accountService;
    private TransactionService transactionService;
    private IConfiguration configuration;


    public HomeController(AccountService _accountService, TransactionService _transactionService, IConfiguration _configuration)
    {
        accountService = _accountService;
        transactionService = _transactionService;
        configuration = _configuration;
    }
    [Route("index")]
    public IActionResult Index()
    {
        var currentAccount = HttpContext.Session.GetString("username");
        var account = accountService.findByUsername(currentAccount);
        ViewBag.Account = account;
        ViewBag.Transaction = transactionService.findTransById(account.AccId);
        return View("index");
    }
    [Route("deposit")]
    public IActionResult Deposit()
    {
        var currentAccount = HttpContext.Session.GetString("username");
        ViewBag.Account = accountService.findByUsername(currentAccount);
        return View("deposit", "home");
    }
    [HttpPost]
    [Route("deposit")]
    public IActionResult Deposit(double money)
    {
        var currentAccount = HttpContext.Session.GetString("username");
        var account = accountService.findByUsername(currentAccount);
        ViewBag.Account = account;
        if (money > 0)
        {
            var transaction = new TransactionDetail
            {
                AccId = account.AccId,
                TransMoney = money,
                TransType = 1,
                DateOfTrans = DateTime.Now
            };
            if (transactionService.Add(transaction))
            {
                account.Balance += money;
                if (accountService.Update(account))
                {
                    var mailHelper = new MailHelper(configuration);

                    var content = "<p>Bạn đã nạp thành công " + money + "Vnđ vào tài khoản " + account.Username + "</p>";
                    content += "<br/> <p>Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi</p>";
                    if (mailHelper.Send("trananhkietzxz@gmail.com", account.Email, "MTBS nạp tiền", content))
                    {
                        TempData["mailMsg"] = "Gửi mail thành công";
                    }
                    else
                    {
                        TempData["mailMsg"] = "Gửi mail thất bại";

                    }
                    TempData["tMsg"] = "Giao dịch thành công";
                    return RedirectToAction("index", "home");
                }
                else
                {
                    TempData["tMsg"] = "Giao dịch thất bại, không thể thêm tiền";

                    return RedirectToAction("deposit", "home");
                }
            }
            else
            {
                TempData["tMsg"] = "Giao dịch thất bại, không thể tạo giao dịch";
                return RedirectToAction("deposit", "home");
            }

        }
        else
        {
            TempData["tMsg"] = "Giao dịch thất bại, số tiền không hợp lệ";
            return RedirectToAction("deposit", "home");
        }

    }

    [Route("withdraw")]
    public IActionResult withdraw()
    {
        var currentAccount = HttpContext.Session.GetString("username");
        ViewBag.Account = accountService.findByUsername(currentAccount);
        return View("withdraw", "home");
    }
    [HttpPost]
    [Route("withdraw")]
    public IActionResult withdraw(double money)
    {
        var currentAccount = HttpContext.Session.GetString("username");
        var account = accountService.findByUsername(currentAccount);
        ViewBag.Account = account;
        if (money > 0 && account.Balance > money)
        {
            var transaction = new TransactionDetail
            {
                AccId = account.AccId,
                TransMoney = money,
                TransType = 2,
                DateOfTrans = DateTime.Now
            };
            if (transactionService.Add(transaction))
            {
                account.Balance -= money;
                if (accountService.Update(account))
                {
                    TempData["tMsg"] = "Giao dịch thành công";
                    var mailHelper = new MailHelper(configuration);
                    var body = "<p>Bạn đã rút thành công " + money + "Vnđ ở tài khoản " + account.Username + "</p> <br/> <p>Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi</p>";
                    if (mailHelper.Send("trananhkietzxz@gmail.com", account.Email, "MTBS rút tiền", body))
                    {
                        TempData["mailMsg"] = "Gửi mail thành công";
                    }
                    else
                    {
                        TempData["mailMsg"] = "Gửi mail thất bại";
                    }
                    return RedirectToAction("index", "home");
                }
                else
                {
                    TempData["tMsg"] = "Giao dịch thất bại, không thể thêm tiền";
                    return RedirectToAction("withdraw", "home");
                }
            }
            else
            {
                TempData["tMsg"] = "Giao dịch thất bại, không thể tạo giao dịch";
                return RedirectToAction("withdraw", "home");
            }

        }
        else
        {
            TempData["tMsg"] = "Giao dịch thất bại, số tiền không hợp lệ";
            return RedirectToAction("withdraw", "home");
        }

    }

    [HttpGet]
    [Route("sort")]
    public IActionResult sort(int transType)
    {
        var currentAccount = HttpContext.Session.GetString("username");
        var account = accountService.findByUsername(currentAccount);
        ViewBag.Account = account;
        if (transType == -1)
        {
            return new JsonResult(transactionService.findTransByIdAjax(account.AccId));
        }
        else
        {
            return new JsonResult(transactionService.findTransByTypeAjax(account.AccId, transType));
        }

    }
}
