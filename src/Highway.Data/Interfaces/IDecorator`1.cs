using System;

namespace Highway.Data
{
	public interface IDecorator<out T>
	{
		T GetRootDecoratedObject();
	}
}