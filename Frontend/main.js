//Populates dropdown menu with all availible words
async function populateDropdownWithWords() {   

    try {
		//Fetch response
		const response = await fetch(`${apiUrl}${getAllWords}`);
		if (!response.ok) {
			console.error('Error fetching words:', response);
			return;
		}		
		//parse response message as json
		const data = await response.json();
		
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
		data.forEach(word => {
			const option = document.createElement('option');
			option.value = word.value;
			option.textContent = word.value;
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
	const synonym = document.getElementById('dropdown').value;

	//Create add synonym dto request object
	const addSynonymRequest = {
		NewWord: newWord,
		Synonym: synonym
	};

  try {
	  //fetch response
    const response = await fetch(`${apiUrl}${addNewWord}`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(addSynonymRequest)
    });

    if (!response.ok) {
		console.error('Error adding new word:', response);
		return;
    }
   
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
	
	const response = await fetch(`${apiUrl}${getSynonyms}`, {
		method: 'POST',
		headers: {
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
	
	const synonyms = synonymsResponse.map(item => item.value);
	const columns = 4;
	let tableHtml = '<table><tbody>';
	
	for (let i = 0; i < synonyms.length; i += columns) {
		tableHtml += '<tr>';
		for (let j = i; j < i + columns && j < synonyms.length; j++) {
		tableHtml += `<td class="td-profile">${synonyms[j]}</td>`;
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