using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using SpecExpress.Rules;

namespace SpecExpress.RuleTree
{
    public class RuleExpressionFactory<T, TProperty>
    {
        public Func<RuleValidatorContext<T, TProperty>, SpecificationContainer, ValidationNotification, bool> CreateExpression(RuleTree<T, TProperty> tree)
        {
            var contextParam = Expression.Parameter(typeof(RuleValidatorContext<T, TProperty>), "context");
            var specContainerParam = Expression.Parameter(typeof(SpecificationContainer), "container");
            var specNotificationParam = Expression.Parameter(typeof(ValidationNotification), "notification");

            var expression = BuildExpression(tree.Root, contextParam, specContainerParam, specNotificationParam);
            var lambda = Expression.Lambda(expression, new ParameterExpression[] { contextParam, specContainerParam, specNotificationParam });
            var compiledFunc = lambda.Compile() as Func<RuleValidatorContext<T, TProperty>, SpecificationContainer, ValidationNotification, bool>;
            return compiledFunc;
        }

        private Expression BuildExpression(NodeBase<T, TProperty> node, ParameterExpression contextParam, ParameterExpression specContainerParam, ParameterExpression specNotificationParam)
        {
            // If node is null, simply return True (as in the rule is valid)
            if (node == null)
            {
                return Expression.Constant(true);
            }

            if (node is RuleNode<T, TProperty>)
            {
                return buildRuleNodeExpression(node as RuleNode<T, TProperty>, contextParam, specContainerParam, specNotificationParam);
            }

            if (node is GroupNode<T, TProperty>)
            {
                return buildGroupNodeExpression(node as GroupNode<T, TProperty>, contextParam, specContainerParam, specNotificationParam);
            }

            return null;
        }

        private Expression buildRuleNodeExpression(RuleNode<T, TProperty> ruleNode, ParameterExpression contextParam, ParameterExpression specContainerParam, ParameterExpression specNotificationParam)
        {

            var validateMethod = ruleNode.Rule.GetType().GetMethod("Validate");
            if (!ruleNode.HasChild)
            {
                return Expression.Call(Expression.Constant(ruleNode.Rule), validateMethod,
                                       contextParam, specContainerParam, specNotificationParam);
            }
            else
            {
                var leftExp = Expression.Call(Expression.Constant(ruleNode.Rule), validateMethod,
                                              contextParam, specContainerParam, specNotificationParam);
                if (ruleNode.ChildHasAndRelationship)
                {
                    if (ruleNode.ChildRelationshipIsConditional)
                    {
                        return Expression.AndAlso(leftExp,
                      BuildExpression(ruleNode.ChildNode, contextParam, specContainerParam,
                                      specNotificationParam));

                    }
                    else
                    {

                        return Expression.And(leftExp,
                                              BuildExpression(ruleNode.ChildNode, contextParam, specContainerParam,
                                                              specNotificationParam));
                    }
                }
                else
                {
                    if (ruleNode.ChildRelationshipIsConditional)
                    {
                        return Expression.OrElse(leftExp,
                                             BuildExpression(ruleNode.ChildNode, contextParam, specContainerParam,
                                                             specNotificationParam));
                    }
                    else
                    {
                        return Expression.Or(leftExp,
                                             BuildExpression(ruleNode.ChildNode, contextParam, specContainerParam,
                                                             specNotificationParam));
                    }
                }
            }
        }

        private Expression buildGroupNodeExpression(GroupNode<T, TProperty> groupNode, ParameterExpression contextParam, ParameterExpression specContainerParam, ParameterExpression specNotificationParam)
        {
            Expression exp = BuildExpression(groupNode.GroupRoot, contextParam, specContainerParam,
                                             specNotificationParam);

            if (!groupNode.HasChild)
            {
                return exp;
            }
            else
            {

                Expression rightExp = BuildExpression(groupNode.ChildNode, contextParam, specContainerParam,
                                                      specNotificationParam);

                if (groupNode.ChildHasAndRelationship)
                {
                    if (groupNode.ChildRelationshipIsConditional)
                    {
                        return Expression.MakeBinary(ExpressionType.AndAlso, exp, rightExp);
                    }
                    else
                    {
                        return Expression.MakeBinary(ExpressionType.And, exp, rightExp);
                    }
                }
                else
                {
                    if (groupNode.ChildRelationshipIsConditional)
                    {
                        return Expression.MakeBinary(ExpressionType.OrElse, exp, rightExp);
                    }
                    else
                    {
                        return Expression.MakeBinary(ExpressionType.Or, exp, rightExp);
                    }
                }
            }
        }

    }
}