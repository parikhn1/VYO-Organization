using System.Collections.Generic;
using VYODomain.Entities;
namespace VYO.Models
{
    public class BhajanDocumentModels
    {
        public List<Document> VYODocumnetExploreList {get;set;}
        public List<BhajanName> BhajanNameList { get; set; }

        public string[] CustomIndexDetail { get; set; }
        public string CustomIndexPage { get; set; }

        public List<CoverPageDocuments> VYOCoverPagesExplorList { get; set; }
    }
}