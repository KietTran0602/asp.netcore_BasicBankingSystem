using HeThongNganHang.Models;

namespace HeThongNganHang.Services;

public interface TransactionService
{
    public List<TransactionDetail> findAll();
    public List<TransactionDetail> findTransById(int id);
    public bool Add(TransactionDetail transaction);

    public dynamic findTransByIdAjax(int id);
    public dynamic findTransByTypeAjax(int id, int transType);


}
