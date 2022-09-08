using Domain.Models;

namespace Services.CustomEventsArguments;

public class MemberRemovedEventArgs : EventArgs
{
    public Member Member { get; init; }

	public MemberRemovedEventArgs(Member member)
	{
		Member = member;
	}
}
