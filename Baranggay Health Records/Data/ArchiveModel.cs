﻿namespace Baranggay_Health_Records.Data
{
    public class ArchiveModel
    {
        public int? ID { get; set; }
        public String? Name { get; set; }

        public String? Date { get; set; }

        public int? GetID()
        {
            return this.ID;
        }

        public String? GetName()
        {
            return this.Name;
        }

        public String? GetDate()
        {
            return this.Date;
        }
    }
}
