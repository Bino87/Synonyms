async function populateDropdownWithWords() {   

    try {
		
		const credentials = `${username}:${password}`;
		const base64Credentials = btoa(credentials);
		//Fetch response
		const response = await fetch(`${apiUrl}${getAllWords}`, {
			headers: {
				Authorization: `Basic ${base64Credentials}`
			}
		});
		if (!response.ok) {
			console.error('Error fetching words:', response);
			return;
		}		
		//parse response message as json
		const data = await response.json();
				
		if(renderErrorCodes(data.errorCodes))
		{
			return;
		}	
		
			// get dropdown menu object
			const dropdown = document.getElementById('dropdown');
	
			// Clear existing options
			dropdown.innerHTML = '';
	
			// Add null option as the first option
			const nullOption = document.createElement('option');
			nullOption.value = '';
			nullOption.textContent = 'Select a word...'; // Customize the text as needed
			dropdown.appendChild(nullOption);
	
			// set each word as a dropdown menu item
			data.response.forEach(word => {
				const option = document.createElement('option');
				option.value = word.value;
				option.textContent = word.value;
				option.dataset.id = word.id;
				dropdown.appendChild(option);
			});	
    } catch (error) {
		
		console.error('Network error:', error);
    } 
  }

//adds new word to the system
async function handleAddWord() {
	//get inputText and dropdown objects
	const newWord = document.getElementById('inputText').value;
	const dropdown = document.getElementById('dropdown');
	const selectedOption = dropdown.options[dropdown.selectedIndex];
	
	let synonym;
	if (selectedOption) {
        synonym = selectedOption.dataset.id; // Get the id from the data attribute      
    }    

	//Create add synonym dto request object
	const addSynonymRequest = {
		Value: newWord,
		SynonymId: synonym
	};

  try {
	  
	const credentials = `${username}:${password}`;
    const base64Credentials = btoa(credentials);
	  //fetch response
    const response = await fetch(`${apiUrl}${addNewWord}`, {
      method: 'POST',
      headers: {
		Authorization: `Basic ${base64Credentials}`,
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(addSynonymRequest)
    });

    if (!response.ok) {		
		return;
    }
	
	const data = await response.json();	
	if(renderErrorCodes(data.errorCodes))
	{
		return;
	}
	// get dropdown menu object
	const dropdown = document.getElementById('synonymsContainer');
	
	// Clear existing options
	dropdown.innerHTML = '';
   
  } catch (error) {
		console.error('Network error:', error);
		return;
  }

	//reload dropdown menu items
	await populateDropdownWithWords();
}

//fetches synonyms of a word
async function fetchSynonyms(word) {
  const dto = {
    Value: word
  };

  try {
	
	const credentials = `${username}:${password}`;
    const base64Credentials = btoa(credentials);
	
	const response = await fetch(`${apiUrl}${getSynonyms}`, {
		method: 'POST',
		headers: {
			Authorization: `Basic ${base64Credentials}`,
			'Content-Type': 'application/json'
		},
		body: JSON.stringify(dto)
	});

    if (!response.ok) {
		console.error('Error fetching synonyms:', response);
		return [];
    }	
	
	//parse response message from json
    const data = await response.json();		
    return data || [];
	
  } catch (error) {
    console.error('Network error:', error);
    return [];
  }
}

//renders a table of synonyms
function renderTable(synonymsResponse) {	
	
	if(renderErrorCodes(synonymsResponse.errorCodes))
	{
		return;
	}
	
	const synonyms = synonymsResponse.response;
	const columns = 4;
	let tableHtml = '<table><tbody>';
	
	for (let i = 0; i < synonyms.length; i += columns) {
		tableHtml += '<tr>';
		for (let j = i; j < i + columns && j < synonyms.length; j++) 
		{	
			const closenessClass = `closeness${synonyms[j].closeness}`; // Get the appropriate class based on closeness
      		tableHtml += `<td class="td-profile ${closenessClass}">${synonyms[j].value}</td>`;
		}
		tableHtml += '</tr>';
	}
	
	tableHtml += '</tbody></table>';
	
	const synonymsContainer = document.getElementById('synonymsContainer');
	synonymsContainer.innerHTML = tableHtml;		
}

//handles fetching of synonyms
async function handleFetchSynonyms() {
 const word = document.getElementById('inputText').value;
  fetchSynonyms(word)
    .then(synonyms => {			
      renderTable(synonyms);			
    });
}

//populate dropdown menu with availible words at start
populateDropdownWithWords();