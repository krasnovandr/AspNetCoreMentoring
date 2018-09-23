using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMentoring.UI.ViewModels.Product;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AspNetCoreMentoring.UI.Validators
{
    public class ProductDiscountValidator : ValidationAttribute
    {
        private readonly int _requiredItemsForDiscount;

        public ProductDiscountValidator(int requiredItemsForDiscount)
        {
            _requiredItemsForDiscount = requiredItemsForDiscount;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var  product = (ProductWriteItemViewModel)validationContext.ObjectInstance;

            if (product.UnitsOnOrder <= _requiredItemsForDiscount && product.Discontinued)
            {
                return new ValidationResult(GetErrorMessage(_requiredItemsForDiscount));
            }

            return ValidationResult.Success;
        }

        private string GetErrorMessage(int requiredItemsForDiscount)
        {
            return $"Only products with {requiredItemsForDiscount} Units in Order are allowed for discount";
        }
    }
}
