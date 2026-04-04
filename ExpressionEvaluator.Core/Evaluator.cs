namespace ExpressionEvaluator.Core;

public class Evaluator
{
    public static double Evaluate(string infix)
    {
        var postfix = InfixToPostfix(infix);
        return EvaluatePostfix(postfix);
    }

    private static string InfixToPostfix(string infix)
    {
        var postFix = string.Empty;
        var stack = new Stack<char>();
        var queue = new Queue<char>();

        foreach (var item in infix)
        {
            if (IsOperator(item))
            {
                if (queue.Count > 0)
                {
                    var numero = string.Empty;
                    while (queue.Count > 0)
                        numero += queue.Dequeue();
                    postFix += numero + " ";
                }

                if (stack.Count == 0 || item == '(')
                {
                    stack.Push(item);
                }
                else if (item == ')')
                {
                    while (stack.Peek() != '(')
                    {
                        postFix += stack.Pop() + " ";
                    }
                    stack.Pop();
                }
                else
                {
                    while (stack.Count > 0 && stack.Peek() != '(' &&
                           PriorityStack(stack.Peek()) >= PriorityInfix(item))
                    {
                        postFix += stack.Pop() + " ";
                    }
                    stack.Push(item);
                }
            }
            else
            {
                queue.Enqueue(item);
            }
        }

        if (queue.Count > 0)
        {
            var numero = string.Empty;
            while (queue.Count > 0)
                numero += queue.Dequeue();
            postFix += numero + " ";
        }

        while (stack.Count > 0)
            postFix += stack.Pop() + " ";

        return postFix.Trim();
    }
    private static int PriorityStack(char item) => item switch
    {
        '^' => 3,
        '*' => 2,
        '/' => 2,
        '+' => 1,
        '-' => 1,
        '(' => 0,
        _ => throw new Exception("Sintax error."),
    };

    private static int PriorityInfix(char item) => item switch
    {
        '^' => 4,
        '*' => 2,
        '/' => 2,
        '+' => 1,
        '-' => 1,
        '(' => 5,
        _ => throw new Exception("Sintax error."),
    };

    private static double EvaluatePostfix(string postfix)
    {
        var stack = new Stack<double>();
        var queue = new Queue<string>(
            postfix.Split(' ', StringSplitOptions.RemoveEmptyEntries));

        while (queue.Count > 0)
        {
            var item = queue.Dequeue();

            if (item.Length == 1 && IsOperator(item[0]))
            {
                var b = stack.Pop();
                var a = stack.Pop();
                stack.Push(item[0] switch
                {
                    '+' => a + b,
                    '-' => a - b,
                    '*' => a * b,
                    '/' => a / b,
                    '^' => Math.Pow(a, b),
                    _ => throw new Exception("Sintax error."),
                });
            }
            else
            {
                stack.Push(double.Parse(item,
                    System.Globalization.CultureInfo.InvariantCulture));
            }
        }

        return stack.Pop();
    }

    private static bool IsOperator(char item) => "+-*/^()".Contains(item);
}