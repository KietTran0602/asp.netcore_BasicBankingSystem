using HeThongNganHang.Models;

namespace HeThongNganHang.Services;

public class AccountServiceImpl : AccountService
{
    private DatabaseContext db;
    public AccountServiceImpl(DatabaseContext _db)
    {
        db = _db;
    }

    public bool Create(Account account)
    {
        try
        {
            db.Accounts.Add(account);
            return db.SaveChanges() > 0;
        }
        catch
        {
            return false;
        }
    }

    public Account findByUsername(string username)
    {
        return db.Accounts.SingleOrDefault(n => n.Username == username);
    }

    public bool Login(string username, string password)
    {
        var account = db.Accounts.SingleOrDefault(n => n.Username == username);
        if (account != null)
        {
            return BCrypt.Net.BCrypt.Verify(password, account.Password);
        }
        return false;
    }

    public bool Update(Account account)
    {
        try
        {
            db.Entry(account).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return db.SaveChanges() > 0;
        }
        catch
        {
            return false;
        }
    }
}
