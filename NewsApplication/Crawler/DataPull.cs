using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Simple.Data;

namespace NewsApplication.Crawler
{
    public class DataPull
    {

        private List<ScoreStructure> Emms
        {
            get
            {
                var scoreStructureList = new List<ScoreStructure>();
                var connectionString = ConfigurationManager.ConnectionStrings["AthenaDb"];
                var db = Database.OpenConnection(connectionString.ConnectionString);
                List<Company> emms = db.MP_MC_GeneralAdmin.All()
                    .Select(
                        db.MP_MC_GeneralAdmin.CompanyName.Distinct()
                    )
                    .Join(db.MP_MF_GeneralAdmin)
                    .On(db.MP_MC_GeneralAdmin.IndexMC == db.MP_MF_GeneralAdmin.IndexMC)
                    .Join(db.LU_ManagementFundInvestmentStatus)
                    .On(db.MP_MF_GeneralAdmin.ManagerFundInvestmentStatus ==
                        db.LU_ManagementFundInvestmentStatus.IndexMFIS)
                    .Where(db.LU_ManagementFundInvestmentStatus.MgrFundInvStatCode == "Emm");
                var connectionStringArticle = ConfigurationManager.ConnectionStrings["NewsApplicationDb"];
                var dbNews = Database.OpenConnection(connectionStringArticle.ConnectionString);
                foreach (var company in emms)
                {
                    var existsInDb = dbNews.Keywords.Find(dbNews.Keywords.Keyword == company.CompanyName);
                    if (existsInDb == null)
                    {
                        dbNews.Keywords.Insert(Keyword: company.CompanyName, Score: 10, Section: "EMM");
                    }
                    var scoreStructure = new ScoreStructure {Keyword = company.CompanyName, Score = 10, Section = "EMM"};
                    scoreStructureList.Add(scoreStructure);
                }
                return scoreStructureList;
            }
        }

        private List<ScoreStructure> Personnel
        {
            get
            {
                var scoreStructureList = new List<ScoreStructure>();
                var db = Database.OpenConnection(ConfigurationManager.ConnectionStrings["AthenaDb"].ConnectionString);
                HashSet<Person> personnel = db.MP_MS_Contact.All()
                    .Select(
                        db.MP_MS_Contact.FirstName,
                        db.MP_MS_Contact.LastName
                    )
                    .Join(db.MP_MF_GeneralAdmin)
                    .On(db.MP_MS_Contact.IndexMF == db.MP_MF_GeneralAdmin.IndexMF)
                    .Join(db.LU_ManagementFundInvestmentStatus)
                    .On(db.MP_MF_GeneralAdmin.ManagerFundInvestmentStatus == db.LU_ManagementFundInvestmentStatus.IndexMFIS)
                    .Join(db.MP_MF_Contact_Employment)
                    .On(db.MP_MS_Contact.IndexContact == db.MP_MF_Contact_Employment.IndexContact)
                    .Join(db.LU_JobTitle)
                    .On(db.MP_MF_Contact_Employment.GCMJobTitle == db.LU_JobTitle.IndexJobTitle)
                    .Where(db.LU_ManagementFundInvestmentStatus.MgrFundInvStatCode == "Emm")
                    .Where(db.MP_MF_Contact_Employment.GCMJobTitle == new[] { "2", "11", "5" })
                    .Where(db.MP_MF_Contact_Employment.DateTerminated == null);
                var connectionStringArticle = ConfigurationManager.ConnectionStrings["NewsApplicationDb"];
                var dbNews = Database.OpenConnection(connectionStringArticle.ConnectionString);
                foreach (var person in personnel.AsEnumerable())
                {
                    var existsInDb = dbNews.Keywords.Find(dbNews.Keywords.Keyword == person.FullName());
                    if (existsInDb == null)
                    {
                        dbNews.Keywords.Insert(Keyword: person.FullName(), Score: 5, Section: "EMM");
                    }
                    var scoreStructure = new ScoreStructure {Keyword = person.FullName(), Score = 5, Section = "EMM"};
                    scoreStructureList.Add(scoreStructure);
                }
                return scoreStructureList;
            }
        }

        private List<ScoreStructure> Keywords
        {
            get
            {
                var connectionString = ConfigurationManager.ConnectionStrings["NewsApplicationDb"];
                var db = Database.OpenConnection(connectionString.ConnectionString);
                return db.Keywords.All()
                    .Select(
                        db.Keywords.Keyword,
                        db.Keywords.Score,
                        db.Keywords.Section
                    );
            }
        }

        private List<ScoreStructure> Clients
        {
            get { return new List<ScoreStructure>(); }
        }

        public List<ScoreStructure> GetData()
        {
            var scores = new List<List<ScoreStructure>> { Keywords, Emms, Personnel, Clients};
            var info = new List<ScoreStructure>();
            foreach (var score in scores)
            {
                foreach (var key in score)
                {
                    if (! info.Contains(key))
                    {
                        info.Add(key);
                    }
                }
            }
            return info;
        }
    }

    public class Company
    {
        public string CompanyName { get; set; }
    }

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName() { return FirstName + " " + LastName;}
    }
}
