effect {
		Name: "Damage" ,
		Params: {
				Amount: Number
				},
				Action: (targets, context) => {
					for target in targets {
						i = 0;
						a = 5;
						while (i < a)
						{
						target.Power -= 1;
						};
					};
				}
			}


			effect {
		Name: "Damageee" ,
		Params: {
				Amount: Number
				},
				Action: (targets, context) => {
					for target in targets {
						i = 0;
						owner = target.Owner;
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
		    owner = target.Owner;
			deck = context.DeckOfPlayer(owner);
			deck.Push(target);
			deck.Shuffle();
			context.Board.Remove(target);
				};
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
						a = 5;
						while (i < Amount)
						{
						target.Power -= 1;
						};
					};
				}
			}




card {
	Type: "Plata",
	Name: "Warrior",
	Faction: "Empire",
	Power: 5,
	Range: ["Melee"],
	OnActivation: [
		{
			Effect: {
				Name: "Damage",
				Amount : 5
			},
			Selector: {
				Source: "board",
				Single: false,
				Predicate: (unit) => unit.Faction == "Empire"
			},
			PostAction: {
				Effect:{
					Name: "ReturnToDeck"
				},
				Selector: {
					Source: "parent",
					Single: false,
					Predicate: (unit) => unit.Power < 1
				},
			}	
		},
		{
			Effect: "Draw",,
		}
	]
}








effect {
		Name: "Damage" ,
		Params: {
				Amount: Number
				},
				Action: (targets, context) => {
					for target in targets {
						i = 0;
						a = 5;
						while (i < Amount)
						{
						target.Power -= 1;
						};
					};
				}
			}



effect {
		Name: "Test" ,
				Action: (targets, context) => {
					d = 6;
				}
			}


card {
	Type: "Plata",
	Name: "Peeeeesssssssssssse",
	Faction: "Empire",
	Power: 5,
	Range: ["Melee"],
	OnActivation: [
		{
			Effect : "Test",,
		}
	]
}

























card {
	Type: "Oro",
	Name: "Beluga",
	Faction: "Northern Realms",
	Power: 10,
	Range: ["Melee"],
	OnActivation: [
		{
			Effect: {
				Name: "Damage",
				Amount : 5,
				Got : gato,
				LET : perro
			},
			Selector: {
				Source: "board",
				Single: false,
				Predicate: (unit) => unit.Faction.Power[5] == "Northern" @@ "Realms"
			},
			PostAction: {
				Effect:{
					Name: "ReturnToDeck"
				},
				Selector: {
					Source: "parent",
					Single: false,
					Predicate: (unit) => unit.Power < 1
				},
			}	
		},
		{
			Effect: "Draw",,
		}
	]
}

card {
	Type: "Oro",
	Name: "Racuda",
	Faction: "Northern Realms",
	Power: 10,
	Range: ["Melee"],
	OnActivation: [
		{
			Effect: {
				Name: "Damage",
				Amount : 5,
				Got : gato,
				LET : perro
			},
			Selector: {
				Source: "board",
				Single: false,
				Predicate: (unit) => unit.Faction.Power[5] == "Northern" @@ "Realms"
			},
			PostAction: {
				Effect:{
					Name: "ReturnToDeck"
				},
				Selector: {
					Source: "parent",
					Single: false,
					Predicate: (unit) => unit.Power < 1
				},
			}	
		},
		{
			Effect: "Draw",,
		}
	]
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
						perro.Hand.Push()[5];
						perro.Hand.Gato[1];
						perro.Hand.TopCard(gato)[5];
						perro.Hand.Lagarto;
						context.Hand[4].Power;
						};
					};
				}
			}