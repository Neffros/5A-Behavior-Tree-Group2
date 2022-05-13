using BehaviorTree;
using Fight.AI.Checks;
using Fight.AI.Tasks;

namespace Fight.AI.Agents
{
	public class BossAI : BehaviorTreeAgent
	{
		protected override Node SetupTree()
		{
			Selector root = new Selector();

			// {
			// 	Sequence jumpSequence = new Sequence();
			// 	root.Attach(jumpSequence);
			// 	
			// 	
			// }

			{
				Sequence blockSequence = new Sequence();
				root.Attach(blockSequence);

				CheckPlayerAttacking checkPlayerAttacking = new CheckPlayerAttacking();
				blockSequence.Attach(checkPlayerAttacking);

				TaskBlock taskBlock = new TaskBlock();
				blockSequence.Attach(taskBlock);
			}
			
			{
				Sequence sequence = new Sequence();
				root.Attach(sequence);

				{
					// Go to player if too far
				
					Repeater repeater = new Repeater
					{
						RepeatMode = RepeatMode.RepeatIfFailure
					};
					sequence.Attach(repeater);

					Selector selector1 = new Selector();
					repeater.Attach(selector1);
				
					CheckPlayerInRange checkPlayerInRange = new CheckPlayerInRange();
					selector1.Attach(checkPlayerInRange);

					TaskGoToPlayer taskGoToPlayer = new TaskGoToPlayer();
					selector1.Attach(taskGoToPlayer);
				}
				
				{
					Selector attackSelector = new Selector();
					sequence.Attach(attackSelector);

					{
						// Attack player with hammer if blocking
						Sequence hammerSequence = new Sequence();
						attackSelector.Attach(hammerSequence);
				
						CheckPlayerBlocking checkPlayerBlocking = new CheckPlayerBlocking();
						hammerSequence.Attach(checkPlayerBlocking);

						TaskAttackHammer taskAttackHammer = new TaskAttackHammer();
						hammerSequence.Attach(taskAttackHammer);
					}
					
					{
						// Attack player with sword else
						TaskAttackSword taskAttackSword = new TaskAttackSword();
						attackSelector.Attach(taskAttackSword);
					}
				}
			}

			return root;
		}
	}
}