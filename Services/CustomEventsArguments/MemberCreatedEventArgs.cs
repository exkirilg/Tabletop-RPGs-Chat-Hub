using Domain.Models;

namespace Services.CustomEventsArguments;

public class MemberCreatedEventArgs : EventArgs
{
	public Member Member { get; init; }

	public MemberCreatedEventArgs(Member member)
	{
		Member = member;
	}
}
