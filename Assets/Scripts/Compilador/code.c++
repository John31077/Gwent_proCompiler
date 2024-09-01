effect {
			Name: "Damage" ,
			Params: 
			{
				Amount: Number
			},
			Action: (targets, context) => 
			{
				1(
			}
		}


		effect {
			Name: "Damageeeeee" ,
			Params: 
			{
				Amount: Number
			},
			Action: (targets, context) => 
			{
				target.Hand.Find((perro) => 5+5)[5].Power = target.Hand[1+1].Find(perro);
			}
		}




		effect {
				Name: "Damage" ,
				Params: {
					Amount: Number
				},
				Action: (targets, context) => {
					for target in targets {
						i = 0;
						while (i < Amount)
						{
						target -= 1;
						};
					};
				}
			}


			effect {
				Name: "Draw",
				Action: (targets, context) => {
					topCard = context.Deck.Pop();
					context.Hand.Add(topCard);
					context.Hand.Shuffle();
				}
			}


			effect {
				Name: "ReturnToDeck",
				Action: (targets, context) => {
					for target in targets {
						owner = target.owner;
						deck = context.DeckOfPlayer(owner);
						deck.Push(target);
						deck.Shuffle();
						context.Board.Remove(target);
					};
				}
			}