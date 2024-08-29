using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Programa
{
	public static void Main(string code)
	{
		LexicalAnalyzer lexical = Compiling.Lexical;
		string text = code;


		IEnumerable<Token> tokens = lexical.GetTokens("code", text, new List<CompilingError>()); //Crea un enumerable de tokens

		foreach (Token token in tokens) //recorre la lista de tokens recien creada e imprime cada token
		{
    		Debug.Log(token);
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//Parsing
		Debug.Log("Parser");

		TokenStream stream = new TokenStream(tokens);
		Parser parser = new Parser(stream);

		//aqui se mete en perro canal. hace todo un arbol raro. evalua elementos, cartas y expresiones super turbias
		List<CompilingError> errors = new List<CompilingError>();

		ElementalProgram program = parser.ParseProgram(errors);




			if (errors.Count > 0)
			{
				foreach (CompilingError error in errors)
				{
					Console.WriteLine("{0}, {1}, {2}", error.Location.Line, error.Code, error.Argument);
					Debug.Log(error.Location.Line + " " + error.Code + " " + error.Argument);
				}
			
			}
































			/*effect {
				Name: "Damage" ,
				Params: {
					Amount: Number
				},
				Action: (targets, context) => {
					for target in targets {
						i = 0;
						while (i++ < Amount)
						target.Power -= 1;
					};
				}
			}

			effect {
				Name: "Draw",
				Action: (target, context) => {
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


						card {
				Type: "Oro",
				Name: "Beluga",
				Faction: "Northern Realms",
				Power: 10,
				Range: ["Melee" , "Ranged" ],
				OnActivation: [
					{
						Effect: "Damage",
						Amount: 5,
					},
					Selector: {
						Source: "board",
						Single: false,
						Predicate: (unit) => unit.Faction == "Northern" && "Realms"
					},
					PostAction: {
						Type: "ReturnToDeck",
						Selector: {
							Source: "parent",
							Single: false,
							Predicate: unit => unit.Power < 1
						},
					}

					{
						Effect: "Draw"
					}
				]
			}*/


	}
}





/*effect {
				Name: "Damage" ,
				Params: {
					Amount: Number
				},
				Action: (targets, context) => {
					for target in targets {
						i = 0;
						while (i++ < Amount)
						target.Power -= 1;
					};
				}
			}

			effect {
				Name: "Draw",
				Action: (target, context) => {
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


						card {
				Type: "Oro",
				Name: "Beluga",
				Faction: "Northern Realms",
				Power: 10,
				Range: ["Melee" , "Ranged" ],
				OnActivation: [
					{
						Effect: "Damage",
						Amount: 5,
					},
					Selector: {
						Source: "board",
						Single: false,
						Predicate: (unit) => unit.Faction == "Northern" && "Realms"
					},
					PostAction: {
						Type: "ReturnToDeck",
						Selector: {
							Source: "parent",
							Single: false,
							Predicate: unit => unit.Power < 1
						},
					}

					{
						Effect: "Draw"
					}
				]
			}*/
