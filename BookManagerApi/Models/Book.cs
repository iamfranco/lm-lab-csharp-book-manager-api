using System.ComponentModel.DataAnnotations.Schema;

namespace BookManagerApi.Models
{
	public class Book
	{
		public long Id { get; set; }

		[Column(TypeName = "text")]
		public string Title { get; set; }

		[Column(TypeName = "text")]
		public string Description { get; set; }

		[Column(TypeName = "text")]
		public string Author { get; set; }
		public Genre Genre { get; set; }
	}
}
