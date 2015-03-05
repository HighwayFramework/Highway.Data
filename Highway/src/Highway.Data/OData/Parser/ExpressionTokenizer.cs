using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;

namespace Highway.Data.OData.Parser
{
    internal static class ExpressionTokenizer
    {
        private static readonly Regex FunctionRx = new Regex(@"^([^\(\)]+)\((.+)\)$", RegexOptions.Compiled);

        private static readonly Regex FunctionContentRx =
            new Regex(
                @"^(.*\((?>[^()]+|\((?<Depth>.*)|\)(?<-Depth>.*))*(?(Depth)(?!))\)|.*?)\s*,\s*(((?<Open>').*(?<Close-Open>')(?(Open)(?!)))|[^,]*)$",
                RegexOptions.Compiled);

        private static readonly Regex AnyAllFunctionRx = new Regex(@"^(([0-9a-zA-Z_/]+/)+)(any|all)\((.*)\)$",
            RegexOptions.Compiled);

        public static ICollection<TokenSet> GetTokens(this string expression)
        {
            var tokens = new Collection<TokenSet>();
            if (string.IsNullOrWhiteSpace(expression))
            {
                return tokens;
            }

            var cleanMatch = expression.EnclosedMatch();

            if (cleanMatch.Success)
            {
                var match = cleanMatch.Groups[1].Value;
                if (!HasOrphanedOpenParenthesis(match))
                {
                    expression = match;
                }
            }

            if (expression.IsImpliedBoolean())
            {
                return tokens;
            }

            var blocks = GetBlocks(expression);

            var openGroups = 0;
            var startExpression = 0;
            var currentTokens = new TokenSet();

            for (var i = 0; i < blocks.Count; i++)
            {
                var netEnclosed = blocks[i].Count(c => c == '(') - blocks[i].Count(c => c == ')');
                openGroups += netEnclosed;

                if (openGroups == 0)
                {
                    if (blocks[i].IsOperation())
                    {
                        var expression1 = startExpression;

                        if (string.IsNullOrWhiteSpace(currentTokens.Left))
                        {
                            var i1 = i;
                            Func<string, int, bool> leftPredicate = (x, j) => j >= expression1 && j < i1;

                            currentTokens.Left = string.Join(" ", blocks.Where(leftPredicate));
                            currentTokens.Operation = blocks[i];
                            startExpression = i + 1;

                            if (blocks[i].IsCombinationOperation())
                            {
                                currentTokens.Right = string.Join(" ", blocks.Where((x, j) => j > i));

                                tokens.Add(currentTokens);
                                return tokens;
                            }
                        }
                        else
                        {
                            var i2 = i;
                            Func<string, int, bool> rightPredicate = (x, j) => j >= expression1 && j < i2;
                            currentTokens.Right = string.Join(" ", blocks.Where(rightPredicate));

                            tokens.Add(currentTokens);

                            startExpression = i + 1;
                            currentTokens = new TokenSet();

                            if (blocks[i].IsCombinationOperation())
                            {
                                tokens.Add(new TokenSet {Operation = blocks[i].ToLowerInvariant()});
                            }
                        }
                    }
                }
            }

            var remainingToken = string.Join(" ", blocks.Where((x, j) => j >= startExpression));

            if (!string.IsNullOrWhiteSpace(currentTokens.Left))
            {
                currentTokens.Right = remainingToken;
                tokens.Add(currentTokens);
            }
            else if (remainingToken.IsEnclosed())
            {
                currentTokens.Left = remainingToken;
                tokens.Add(currentTokens);
            }
            else if (tokens.Count > 0)
            {
                currentTokens.Left = remainingToken;
                tokens.Add(currentTokens);
            }

            return tokens;
        }

        public static TokenSet GetArithmeticToken(this string expression)
        {
            var cleanMatch = expression.EnclosedMatch();

            if (cleanMatch.Success)
            {
                var match = cleanMatch.Groups[1].Value;
                if (!HasOrphanedOpenParenthesis(match))
                {
                    expression = match;
                }
            }

            var blocks = expression.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            var hasOperation = blocks.Any(x => x.IsArithmetic());
            if (!hasOperation)
            {
                return null;
            }

            var operationIndex = GetArithmeticOperationIndex(blocks);

            var left = string.Join(" ", blocks.Where((x, i) => i < operationIndex));
            var right = string.Join(" ", blocks.Where((x, i) => i > operationIndex));
            var operation = blocks[operationIndex];

            return new TokenSet {Left = left, Operation = operation, Right = right};
        }

        public static TokenSet GetAnyAllFunctionTokens(this string filter)
        {
            var functionMatch = AnyAllFunctionRx.Match(filter);
            if (!functionMatch.Success)
            {
                return null;
            }

            var functionCollection = functionMatch.Groups[1].Value.Trim('/');
            var functionName = functionMatch.Groups[3].Value;
            var functionContent = functionMatch.Groups[4].Value;

            return new FunctionTokenSet
            {
                Operation = functionName,
                Left = functionCollection,
                Right = functionContent
            };
        }

        public static TokenSet GetFunctionTokens(this string filter)
        {
            var functionMatch = FunctionRx.Match(filter);
            if (!functionMatch.Success)
            {
                return null;
            }

            var functionName = functionMatch.Groups[1].Value;
            var functionContent = functionMatch.Groups[2].Value;
            var functionContentMatch = FunctionContentRx.Match(functionContent);
            if (!functionContentMatch.Success)
            {
                return new FunctionTokenSet
                {
                    Operation = functionName,
                    Left = functionContent
                };
            }

            return new FunctionTokenSet
            {
                Operation = functionName,
                Left = functionContentMatch.Groups[1].Value,
                Right = functionContentMatch.Groups[2].Value
            };
        }

        private static int GetArithmeticOperationIndex(IList<string> blocks)
        {
            var openGroups = 0;
            var operationIndex = -1;
            for (var i = 0; i < blocks.Count; i++)
            {
                var source = blocks[i];

                Contract.Assume(source != null, "Does not generate null token parts.");

                var netEnclosed = source.Count(c => c == '(') - source.Count(c => c == ')');
                openGroups += netEnclosed;

                if (openGroups == 0 && source.IsArithmetic())
                {
                    operationIndex = i;
                }
            }

            return operationIndex;
        }

        private static bool HasOrphanedOpenParenthesis(string expression)
        {
            var opens = new List<int>();
            var closes = new List<int>();
            var index = expression.IndexOf('(');
            while (index > -1)
            {
                opens.Add(index);
                index = expression.IndexOf('(', index + 1);
            }

            index = expression.IndexOf(')');
            while (index > -1)
            {
                closes.Add(index);
                index = expression.IndexOf(')', index + 1);
            }

            var pairs = opens.Zip(closes, (o, c) => new Tuple<int, int>(o, c));
            return opens.Count == closes.Count && pairs.Any(x => x.Item2 < x.Item1);
        }

        /// <summary>
        ///     Splits <paramref name="str" /> by spaces where the spaces are not contained in single-quoted strings.
        ///     Empty blocks excluded from returned list.
        /// </summary>
        private static IList<string> GetBlocks(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return new List<string>();
            }

            var blocks = new List<string>();
            var blockStartPos = 0;
            var stringQuoteCount = 0;
            var insideString = false;
            var pos = 0;

            for (; pos < str.Length; pos++)
            {
                if (str[pos] == '\'')
                {
                    stringQuoteCount++;
                    if (!insideString)
                    {
                        insideString = true;
                    }
                    else if (stringQuoteCount%2 == 0 && (str.Length == (pos + 1) || str[(pos + 1)] != '\''))
                    {
                        // if we're at an even number of single quotes so far and the next character
                        // is not a single quote, then we've reached the end of the string
                        insideString = false;
                    }
                }
                else if (str[pos] == ' ' && !insideString)
                {
                    if (pos > 0 && str[pos - 1] != ' ')
                    {
                        // we've reached the end of block if the current character is a space and the previous character wasn't
                        blocks.Add(str.Substring(blockStartPos, pos - blockStartPos));
                    }

                    blockStartPos = pos + 1;
                }
            }

            if (str[pos - 1] != ' ')
            {
                // if we've ended on a space, we've already added the last block
                blocks.Add(str.Substring(blockStartPos));
            }

            return blocks;
        }
    }
}