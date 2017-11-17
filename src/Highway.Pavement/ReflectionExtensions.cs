using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Highway.Pavement
{
	public static class ReflectionExtensions
	{
		public static async Task<object> InvokeAsync(this MethodInfo methodInfo, object obj, params object[] parameters)
		{
			var task = (Task)methodInfo.Invoke(obj, parameters ?? new object[] { });
			await task.ConfigureAwait(false);
			var resultProperty = task.GetType().GetProperty("Result");
			return resultProperty.GetValue(task);
		}
	}
}