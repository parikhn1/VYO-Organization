using System.Data;
using System.Collections.Generic;
using VYODAL.Infrastructure;
using MySql.Data.MySqlClient;
using VYODomain.Entities;

namespace VYODAL.DB.Repository
{
    public class VYODocumentsExploreRepository : BaseRepository
    {

        //public List<Document> GetDocuments()
        //{
        //    List<Document> DocumentList = new List<Document>();
        //    Document doc = new Document();
        //    SetConnection();
        //    using (MySqlCommand cmd = new MySqlCommand("GetDocuments", Connection))
        //    {
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        MySqlDataReader reader = cmd.ExecuteReader();
        //        var t = ObjectContext.Translate<Document>(reader).toList();

        //        int i = reader.FieldCount;

        //        if (reader.Read())
        //        {
        //            VYODomain.Entities.Document DocObj = new VYODomain.Entities.Document();
        //            DocObj.SpecificationName = reader["Specification_Name"].ToString();
        //            DocObj.ClassificationName = reader["ClassficationName"].ToString();
        //            DocObj.ItemName = reader["itemlist"].ToString();
        //            DocumentList.Add(DocObj);
        //        }

        //        reader.Close();
        //        return DocumentList;
        //    }
        //}

        public List<CoverPageDocuments> GetCoverPages()
        {
            List<CoverPageDocuments> DocumentList = new List<CoverPageDocuments>();
            Document doc = new Document();
            SetConnection();
            using (MySqlCommand cmd = new MySqlCommand("GetCoverPages", Connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                MySqlDataReader reader = cmd.ExecuteReader();
                //var t = ObjectContext.Translate<Document>(reader).toList();

                int i = reader.FieldCount;

                while (reader.Read())
                {
                    CoverPageDocuments DocObj = new CoverPageDocuments();
                   
                    DocObj.CoverPageName = reader["CoverPageName"].ToString();
                    DocumentList.Add(DocObj);
                }

                CloseConnection();
                reader.Close();
                return DocumentList;
            }
        }

        public List<Document> GetDocuments()
        {
            List<Document> DocumentList = new List<Document>();
            Document doc = new Document();
            SetConnection();
            using (MySqlCommand cmd = new MySqlCommand("GetDocuments", Connection))
            {
                
                cmd.CommandType = CommandType.StoredProcedure;
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    int i = reader.FieldCount;

                    while (reader.Read())
                    {
                        Document DocObj = new Document();
                        DocObj.ItemID = reader["Items_Id"].ToString();
                        DocObj.SpecificationName = reader["Specification_Name"].ToString();
                        DocObj.ClassificationName = reader["ClassficationName"].ToString();
                        DocObj.ItemName = reader["itemlist"].ToString();
                        DocumentList.Add(DocObj);
                    }

                    reader.Close();
                }
                CloseConnection();
                return DocumentList;
            }
        }
        public bool AddBhajan(int SpecificationId, int ClassificationId, string BhajanName)
        {
            SetConnection();
            using (MySqlCommand cmd = new MySqlCommand("AddDocument", Connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Specification_Id", SpecificationId);
                cmd.Parameters.AddWithValue("@Classification_Id", ClassificationId);
                cmd.Parameters.AddWithValue("@ItemList", BhajanName);

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    CloseConnection();
                    return false;
                }
            }
            CloseConnection();
            return true;
            }

        public bool AddCoverPage(string CoverPageName)
        {
            SetConnection();
            using (MySqlCommand cmd = new MySqlCommand("AddCoverPage", Connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
              
                cmd.Parameters.AddWithValue("@CoverPageName", CoverPageName);

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    CloseConnection();
                    return false;
                }
            }
            CloseConnection();
            return true;
        }
    }
}