using Domain.Models;

namespace Services.CustomEventsArguments;

public class MemberChangedEventArgs : EventArgs
{
	public Member Member { get; init; }

	public MemberChangedEventArgs(Member member)
	{
		Member = member;
	}
}
