using System.Xml.Linq;

namespace Baranggay_Health_Records.Data
{
    public class IllnessModel
    {
        public int ID { get; set; }
        public string IllnessName { get; set; } = "";

        public string RecommendedMedicine { get; set; } = "";

        public int MedicineID { get; set; }


        public int Occurence { get; set; }

        public int GetID()
        {
            return this.ID;
        }

        public string GetName()
        {
            return this.IllnessName;
        }

        public string GetRecommendedMedicine()
        {
            return this.RecommendedMedicine;
        }

        public int GetOccurence()
        {
            return this.Occurence;
        }

    }
}


