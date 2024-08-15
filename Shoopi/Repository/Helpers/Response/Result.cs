using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Helpers.Response
{
	public class Result<T>
	{
		public bool Success { get; set; }
		public T? Data { get; set; }
		public string ErrorMessage { get; set; } = string.Empty;

	}
}
