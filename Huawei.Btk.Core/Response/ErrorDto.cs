using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huawei.Btk.Core.Response
{
	public class ErrorDto
	{
		public List<string> Errors { get; private set; }

		public ErrorDto(string error)
		{
			Errors = new List<string>() { error };
		}

		public ErrorDto(List<string> errors)
		{
			Errors = errors;
		}
	}
}
