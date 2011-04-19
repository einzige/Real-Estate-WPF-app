using System.Linq;

namespace MyCompany
{
    using Infrastructure.Estate;
    using EstateDataAccess;
    using System;
    using System.Collections.Generic;

    public static class AgencyModel
    {
        #region Agency
        public static Agency GetAgency(int agencyID)
        {
            EstateDataAccess.EstateCompact ec = new EstateCompact(DatabaseInfo.connectionString);
            return (from a in ec.Agencies where a.ID == agencyID select a).First<Agency>();
        }


        public static void UpdateAgency(Agency updated)
        {
            EstateDataAccess.EstateCompact ec = new EstateCompact(DatabaseInfo.connectionString);

            Agency current = (from a in ec.Agencies where a.ID == updated.ID select a).First<Agency>();

            current.About = updated.About;
            current.Address = updated.Address;
            current.Enabled = updated.Enabled;
            current.Foundation = updated.Foundation;
            current.ICQ = updated.ICQ;
            current.LastUpdateTime = DateTime.Now;
            current.Logo = updated.Logo;
            current.Mail = updated.Mail;
            current.MapUrl = updated.MapUrl;
            current.Name = updated.Name;
            current.Passage = updated.Passage;
            current.Phone = updated.Phone;
            current.SiteUrl = updated.SiteUrl;
            current.Skype = updated.Skype;

            ec.SubmitChanges();
        }
        #endregion

        #region Realtor
        public static List<Realtor> GetRealtors(int? agencyID)
        {
            EstateDataAccess.EstateCompact ec = new EstateCompact(DatabaseInfo.connectionString);

            if(agencyID != null)
                return (from r in ec.Realtors where r.AgencyID == agencyID select r).ToList<Realtor>();

            return (from r in ec.Realtors select r).ToList<Realtor>();
        }

        public static Realtor GetRealtor(int? realtorID)
        {
            if (realtorID == null || realtorID == 0) return new Realtor();

            EstateDataAccess.EstateCompact ec = new EstateCompact(DatabaseInfo.connectionString);
            return (from r in ec.Realtors where r.ID == realtorID select r).First<Realtor>();
        }

        public static void UpdateRealtor(Realtor updated)
        {
            EstateDataAccess.EstateCompact ec = new EstateCompact(DatabaseInfo.connectionString);

            #region UPDATE QUERY
            if (updated.ID != 0)
            {
                Realtor current = (from r in ec.Realtors where r.ID == updated.ID select r).First<Realtor>();

                current.AgencyID = updated.AgencyID;
                current.Appointment = updated.Appointment;
                current.Birthday = updated.Birthday;
                current.Commencement = updated.Commencement;
                current.ContactPhone = updated.ContactPhone;
                current.Discharge = updated.Discharge;
                current.Enabled = updated.Enabled;
                current.FIO = updated.FIO;
                current.ICQ = updated.ICQ;
                current.Info = updated.Info;
                current.LastUpdateTime = DateTime.Now;
                current.Login = updated.Login;
                current.Mail = updated.Mail;
                current.OfficePhone = updated.OfficePhone;
                current.Password = updated.Password;
                current.PermissionID = updated.PermissionID;
                current.Photo = updated.Photo;
                current.Remuneration = updated.Remuneration;
                current.Skype = updated.Skype;

                ec.SubmitChanges();
                return;
            }
            #endregion
            #region INSERT QUERY

            updated.AgencyID = 1; // FIX: id
            updated.LastUpdateTime = updated.CreationTime = DateTime.Now;
            ec.Realtors.InsertOnSubmit(updated);
            ec.SubmitChanges();

            #endregion
        }
        #endregion
    }
}