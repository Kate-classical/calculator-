using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project1._1
{
	class Parsing
	{
		public const char END_ARG = ')';
		public const char START_ARG = '(';
		public const char END_LINE = '!';

		static int nomerChislo = 0;
		static int nomerUnknown = 0;
		static int nomerOperation = 0;

		public static List<Cell> process(string data)
		{
			List<Cell> listToken = BreakToken(data);
			listToken = reflactorData(listToken);
			listToken = RedactionToPolsk(listToken);
			listToken = RedactioData(listToken);
			return listToken;
		}

		static double Work(double token, double ch, Cell operation) //выполнение слияния
		{
			switch (operation.Token)
			{
				case "˄":
					return  Math.Pow(token, ch);
				case "*":
					return token * ch;
				case "/":
					return token / ch;
				case "+":
					return token + ch;
				case "-":
					return token - ch;
			}
			return 0;
		}

		static List<Cell> RedactioData(List<Cell> listToken)
		{
			List<Cell> newListToken = new List<Cell>();
			Cell ch; 
			List<Cell> stack = new List<Cell>();
			string stringToken = "";
			//for (int i = 0; i < listToken.Count; i++)
			while (listToken.Count != 0)
			{
				Cell token = listToken.First();

				while(token.Type != "operation")
				{
					stack.Add(token);
					listToken.Remove(listToken.First());
					token = listToken.First();
				}

				ch = stack.Last();
				stack.Remove(ch);
				if (token.Token == "=")
				{			
					stack.Add(ch);
					stack.Add(token);
					stack.Add(listToken.Last());
					listToken.Remove(token);
					listToken.Remove(listToken.First());
				}
				else
				{
					if (stack.Count == 0)
					{
						stack.Add(ch);
						stack.Add(token);
						
						listToken.Remove(ch);
						listToken.Remove(listToken.Last());
					}
					else
					{
						if (stack.Last().Type == "digit" && ch.Type == "digit")
						{
							stringToken = Work(Convert.ToInt32(stack.Last().Token), Convert.ToInt32(ch.Token), token).ToString();
							stack.Remove(stack.Last());
							stack.Add(new Cell(stringToken, "digit", 0));
							listToken.Remove(token);
						}
						else
						{
							stringToken = stack.Last().Token + token.Token + ch.Token;
							stack.Remove(stack.Last());
							stack.Add(new Cell(stringToken, "letter", 0));
							listToken.Remove(token);
							stringToken = "";
						}
					}

				}
														
				
			}

			return stack;
		}

		static List<Cell> RedactionToPolsk(List<Cell> listToken)
		{
			Stack<Cell> stack = new Stack<Cell>();
			List<Cell> newListToken = new List<Cell>();
			Cell token;
			for (int i = 0; i < listToken.Count; i++)
			{
				token = listToken[i];
				if (token.Type != "operation")
				{
					newListToken.Add(token);
				}
				else
				{
					if (token.Token == "=")
					{
						while (stack.Count != 0)
						{
							newListToken.Add(stack.Pop());
						}
						while (token.Token != "!")
						{
							newListToken.Add(token);
							token = listToken[++i];
						}
						break;
					}
					
						if (stack.Count == 0)
						{
							stack.Push(token);
						}
						else
						{
							if (GetPrioritet(stack.Peek(), token))
							{
								stack.Push(token);
							}
							else
							{
								newListToken.Add(stack.Pop());
								stack.Push(token);
							}
						}
					
				}
			}
			
			return newListToken;
		}

		static int GetPriority(string action) //расчет приоритетов
		{
			switch (action)
			{
				case "+":
				case "-":
					return 1;
				default:
					return 2;
			}
		}
		static bool GetPrioritet(Cell stak, Cell znach)
		{
			if (GetPriority(stak.Token) < GetPriority(znach.Token))
			{
				return true;
			}
			return false;
		}

		static List<Cell> BreakToken(string data)
		{
			int countParentthese = 0;
			List<Cell> listToken = new List<Cell>();
			string allToken = "";

			foreach (var ch in data)
			{
				if (Char.IsDigit(ch))
				{
					allToken += ch;
					continue;
				}
				if (Char.IsLetter(ch))
				{
					allToken += ch;
					continue;
				}
				else
				{
					if (allToken.Length != 0)
					{
						if (Char.IsLetter(allToken[0]))
						{
							listToken.Add(new Cell(allToken, "letter", ++nomerUnknown));
						}
						else
						{
							listToken.Add(new Cell(allToken, "digit", ++nomerChislo));
						}
					}
					if (ch == '(')
					{
						countParentthese++;
						listToken.Add(new Cell(ch.ToString() + countParentthese.ToString(), "operation", ++nomerOperation));
					}
					else
					{
						if (ch == ')')
						{							
							listToken.Add(new Cell(ch.ToString() + countParentthese.ToString(), "operation", ++nomerOperation));
							countParentthese--;
							allToken = "";
						}
						else
						{
							listToken.Add(new Cell(ch.ToString(), "operation", ++nomerOperation));
							allToken = "";
						}
					}					
				}
			}
			return listToken;
		}

		static List<Cell> reflactorData(List<Cell> listToken)
		{
			List<Cell> stackBefore = new List<Cell>();
			List<Cell> stackAfter = new List<Cell>();
			List<int> elemenWhereRedaction = new List<int>();
			List<int> elemenWhereRedaction2 = new List<int>();
			int countParentthese = 0;
			int countParentthese2 = 0;
			int znatIndex = 100;
			//foreach(var token in listToken)
			for (int i = 0; i < listToken.Count; i++)
			{
				var token = listToken[i];
				if (token.Type == "operation")
				{					
					if (token.Token[0] == '(')
					{
						countParentthese = Convert.ToInt32(token.Token.Remove(0,1));
						bool yes = false;
						listToken.Remove(token);  //delete (
						while ( i != 0)
						{
							token = listToken[--i];
							switch (token.Token)
							{
								case "+":
								case "-":
								case "=":
									{
										yes = true;
										token = listToken[++i];
									}
									break ;								
							}
							if (yes)
								break;
							
							stackBefore.Add(token);
							listToken.RemoveAt(i);
						}
						yes = false;  

						string strokaafter = ')' + countParentthese.ToString();
						token = listToken[i];
						while (token.Token != strokaafter)
						{		
							if (znatIndex == 100)
							{
								znatIndex = i;
							}
							//рекурсия на скобку!!!
							if (getOperation(token.Token))
							{
								elemenWhereRedaction.Add(znatIndex);
								elemenWhereRedaction2.Add(znatIndex);
								znatIndex = 100;
							}
							
							token = listToken[++i];							
						}

						elemenWhereRedaction.Add(znatIndex);
						elemenWhereRedaction2.Add(znatIndex);

						listToken.Remove(token);    //delete )
						//i--;
						while (i != listToken.Count - 1)
						{
							token = listToken[i];
							switch (token.Token)
							{
								case "+":
								case "-":
								case "=":
									yes = true;
									token = listToken[++i];
									break;
							}
							if (yes)
								break;

							stackAfter.Add(token);
							listToken.RemoveAt(i);
							
						}

					}
				}
			}

			int q = 0;

			for (int i = 0; i < listToken.Count; i++)  // before
			{
				if (elemenWhereRedaction.Count != 0)
				{
					if (i == elemenWhereRedaction.First())
					{
						for (int j = 0; j < stackBefore.Count; j++)
						{
							listToken.Insert(i, stackBefore[j]);
						}
						elemenWhereRedaction.RemoveAt(0);
						if (elemenWhereRedaction.Count != 0)
						{
							elemenWhereRedaction[0] += stackBefore.Count;
						}
						else
						{
							break;
						}
					}
				}
				
			}


			for (int i = 0; i < listToken.Count; i++)  // after
			{
				if (elemenWhereRedaction2.Count != 0)
				{
					if (i == elemenWhereRedaction2.First())
					{
						for (int j = 0; j < stackAfter.Count; j++)
						{
							listToken.Insert(++i, stackAfter[j]);
						}
						elemenWhereRedaction2.RemoveAt(0);
						if (elemenWhereRedaction2.Count != 0)
						{
							elemenWhereRedaction2[0] += stackAfter.Count;
						}
						else
						{
							break;
						}
					}
				}

			}

			return listToken;
		}

		static bool getOperation(string ch)
		{
			return ch == "+" || ch == "-" || ch == "=";
		}
	}
}
