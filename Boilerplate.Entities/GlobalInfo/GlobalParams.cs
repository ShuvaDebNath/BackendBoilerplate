using System.Collections.Generic;

namespace Boilerplate.Entities.GlobalInfo
{
    public static class GlobalParams
    {
        #region VC Type
        public static string OpeningVoucherId { get; set; } = "5605B306-1B9C-44DC-9E23-3AE9E98B016C";
        public static string DrvoucherId { get; set; } = "D7965C37-43BF-403B-8476-36CDBD61919E";
        public static string CrvoucherId { get; set; } = "6026D43F-CF52-480F-B1C3-6BE086C91E1D";
        public static string JvVoucherId { get; set; } = "F7735663-AB59-4639-977D-462619907C58";
        public static string CovVoucherId { get; set; } = "9E8EC0AD-210D-4105-960A-E3695C791897";
        #endregion

        #region Ledger_Id
        public static string AccountsPayableId { get; set; } = "5dac230c-7893-4ffc-a704-f733439fc722";
        public static string OthersPayableId { get; set; } = "9391a967-786b-476c-bdf7-c1b69a31ec9d";
        public static string AccountsReceivableId { get; set; } = "c1eafd65-398f-499f-9b8b-497ee335cefb";
        #endregion

        #region Mail settings
        public static string MailSender { get; set; } = "malihastore@malihabd.com";
        public static string MailPassword { get; set; } = "Maliha@570";
        public static string MailReceiver { get; set; } = "agupta@gsciservices.com";
        public static string[] MailCC { get; set; } = { "malihapsfacc@gmail.com" };
        public static List<string> MailBCC { get; set; }
        #endregion

        #region UserTypeId
        public static string UserTypeId_User { get; set; } = "A7CD7E99-8A86-44AD-BC07-9CF36F00C646";
        public static string UserTypeId_Admin { get; set; } = "03D81895-5025-4EB9-92AB-78C3C1E43051";
        public static string UserTypeId_StoreManager { get; set; } = "1879F63F-7168-4B82-B505-FF8F8DBE3776";
        public static string UserTypeId_AccountManager { get; set; } = "6879D3C3-C6CC-417D-8381-A9500B92C334";
        public static string UserTypeId_SuperAdmin { get; set; } = "7A9FE6EB-B545-4AF5-A4C5-557A1EE524EC";
        #endregion

        #region UserId
        public static string AspNetUser_SuperAdmin { get; set; } = "2a72e406-15a9-4bb5-983b-a44cb0bd7323";
        public static string AspNetUser_Shahid { get; set; } = "9d93b86a-4898-4abf-8dda-a23a193484a6";
        public static string AspNetUser_Mamun { get; set; } = "e72303c3-e6c0-4326-bcfb-4c3663549155";
        #endregion

        public static string CompanyId { get; set; } = "bfd4ca78-3ec9-4798-8be9-5a9423917949";

        #region Others
        public static string InvalidTokenMsg { get; set; } = "Invalid Token";
        public static string SuccessMsg { get; set; } = "Success";
        public static string FailMsg { get; set; } = "Fail";
        public static string GetSuccessMsg { get; set; } = "Data get successfully.";
        public static string GetFailMsg { get; set; } = "Data get fail.";
        public static string SaveSuccessMsg { get; set; } = "Save Success";
        public static string SaveFaildMsg { get; set; } = "Save Faild";
        public static string UpdateSuccessMsg { get; set; } = "Update Success";
        public static string UpdateFailMsg { get; set; } = "Update Fail";
        public static string DataExistMsg { get; set; } = "Data already exist";
        public static string DataNotExistMsg { get; set; } = "Data does not exist";
        public static string DeleteMsg { get; set; } = "Data Deleted";
        public static string DeleteSuccessMsg { get; set; } = "Data Delete success";
        public static string DeleteFailMsg { get; set; } = "Data Delete faild";
        public static string LoginFailMsg { get; set; } = "Login Fail";
        public static string LoginSuccessMsg { get; set; } = "Login Success";
        public static string InvalidUserMsg { get; set; } = "Invalid User";
        public static string DateFormateddMMyyyy { get; set; } = "dd/MM/yyyy";
        public static string DateFormateyyyyMMdd { get; set; } = "yyyy-MM-dd";
        public static string UserId { get; set; } = "UserId";
        public static string UserName { get; set; } = "UserName";
        public static string FaildMsg { get; set; } = "faild";
        public static string LedgerSaveSuccessMsg { get; set; } = "Ledger Save Success.";
        public static string LedgerDeleteSuccessMsg { get; set; } = "Ledger Delete Success.";
        public static string DefaultDate { get; set; } = "01/01/2021";
        public static string DefaultDate2021 { get; set; } = "2021-01-01";
        public static string DefaultDate1900 { get; set; } = "1900-01-01";
        public static string DataNotFoundMsg { get; set; } = "Data not Found!";
        #endregion

        #region Response Code
        public static string Code600 { get; set; } = "600";
        public static string Code601 { get; set; } = "601";
        public static string Code602 { get; set; } = "602";
        public static string Code603 { get; set; } = "603";
        public static string Code604 { get; set; } = "604";
        #endregion
    }

    public class DDLItem
    {
        public string DisplayItem { get; set; }
        public string ValueItem { get; set; }
    }
}
