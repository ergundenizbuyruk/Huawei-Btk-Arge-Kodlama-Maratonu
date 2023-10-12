namespace Huawei.Btk.Core.Models
{
	public class Analysis
	{
		public int Id { get; set; }
		public string Result { get; set; }
		public User User { get; set; }
		public Guid UserId { get; set; }

        public string ImagePath { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}