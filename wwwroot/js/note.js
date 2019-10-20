document.addEventListener('DOMContentLoaded', function (evt) {
    const categoryNameInput = document.getElementById('category-name');
    const addCategoryButton = document.getElementById('add-category-button');
    const removeCategoryButton = document.getElementById('remove-category-button');
    const categoryList = document.getElementById('category-list');

    const addCategory = (event) => {
        event.preventDefault();

        const value = categoryNameInput.value.toLowerCase();
        const pTagValues = categoryList.querySelectorAll('p');
        let categoryExists = false;

        pTagValues.forEach((pTag) => {
            if (pTag.innerHTML.toLowerCase() === value.toLowerCase()) {
                categoryExists = true;
            }
        });

        if (!categoryExists) {
            categoryList.innerHTML += `
            <p>${value}</p>
            <input type="hidden" value="${value}" name="categories[]">
        `
        }
        categoryNameInput.value = '';
    };

    const removeCategory = (event) => {
        event.preventDefault();

        const value = categoryNameInput.value;
        const pTagValues = categoryList.querySelectorAll('p');
        const inputTagValues = categoryList.querySelectorAll('input');

        pTagValues.forEach((pTag) => {
            if (pTag.innerHTML.toLowerCase() === value.toLowerCase()) {
                categoryList.removeChild(pTag);
            }
        });

        inputTagValues.forEach((input) => {
            if (input.value.toLowerCase() === value.toLowerCase()) {
                categoryList.removeChild(input)
            }
        });

        categoryNameInput.value = '';
    };

    addCategoryButton.addEventListener('click', addCategory);
    removeCategoryButton.addEventListener('click', removeCategory);
});
