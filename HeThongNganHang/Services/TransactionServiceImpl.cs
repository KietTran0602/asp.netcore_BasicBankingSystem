using HeThongNganHang.Models;

namespace HeThongNganHang.Services;

public class TransactionServiceImpl : TransactionService
{
    private DatabaseContext db;
    public TransactionServiceImpl(DatabaseContext _db)
    {
        db = _db;
    }

    public bool Add(TransactionDetail transaction)
    {
        try
        {
            db.TransactionDetails.Add(transaction);
            return db.SaveChanges() > 0;
        }
        catch
        {
            return false;
        }
    }

    public List<TransactionDetail> findAll()
    {
        return db.TransactionDetails.OrderByDescending(t => t.DateOfTrans).ToList();
    }

    public List<TransactionDetail> findTransById(int id)
    {
        return db.TransactionDetails.Where(t => t.AccId == id).OrderByDescending(t => t.DateOfTrans).OrderByDescending(t => t.TransId).ToList();
    }

    public dynamic findTransByIdAjax(int id)
    {
        return db.TransactionDetails.Where(t => t.AccId == id).OrderByDescending(t => t.DateOfTrans).OrderByDescending(t => t.TransId).Select(t => new
        {
            transmoney = t.TransMoney,
            transtype = t.TransType,
            dateoftrans = t.DateOfTrans.ToString("dd/MM/yyyy")
        }).ToList();
    }

    public dynamic findTransByTypeAjax(int id, int transType)
    {
        return db.TransactionDetails.Where(t => t.AccId == id && t.TransType == transType).OrderByDescending(t => t.DateOfTrans).OrderByDescending(t => t.TransId).Select(t => new
        {
            transmoney = t.TransMoney,
            transtype = t.TransType,
            dateoftrans = t.DateOfTrans.ToString("dd/MM/yyyy")
        }).ToList();
    }
}
