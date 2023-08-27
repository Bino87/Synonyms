function renderErrorCodes(errorCodes) 
{
	const errorContainer = document.getElementById('errorContainer');
	console.log(errorCodes.length);
	if (errorCodes.length === 0) {
		errorContainer.innerHTML = ''; // Clear the container
		
		return false;
	}
	
	// Clear previous content
	errorContainer.innerHTML = '';
	
	// Render each error code as red text on a separate line
	errorCodes.forEach((errorCode) => {
		const errorElement = document.createElement('div');
		errorElement.style.color = 'red';
		errorElement.textContent = getErrorMessage(errorCode);
		errorContainer.appendChild(errorElement);
	});
	
	return true;
}

        // Function to get the error message based on the error code
function getErrorMessage(errorCode) 
{
    switch (errorCode) {
        case 1:
            return 'Validation failed because word had no value';
        case 2:
            return 'Validation failed because word container has illegal characters';
        case 3:
            return 'Unable to retrieve data';
        case 4:
            return 'Word already exists';
        case 5:
            return 'Word does not exist';
        case 6:
            return 'Synonym does not exist';
        default:
            return `Unknown error with code: ${errorCode}`;
    }
}