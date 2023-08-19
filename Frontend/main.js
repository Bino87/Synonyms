async function populateDropdownWithWords() {
    const dropdown = document.getElementById('dropdown');

    try {
      const response = await fetch(`${apiUrl}${getAllWords}`);
      if (!response.ok) {
        console.error('Error fetching words:', response);
        return;
      }
      const data = await response.json();

			// Clear existing options
    	dropdown.innerHTML = '';

			 // Add null option as the first option
    	const nullOption = document.createElement('option');
    	nullOption.value = '';
    	nullOption.textContent = 'Select a word...'; // Customize the text as needed
    	dropdown.appendChild(nullOption);

      data.forEach(word => {
        const option = document.createElement('option');
        option.value = word.value;
        option.textContent = word.value;
        dropdown.appendChild(option);
      });

			console.log('Dropdown refreshed with words:', data);

    } catch (error) {
      console.error('Network error:', error);
    }
  }

async function handleAddWord() {
  const newWord = document.getElementById('inputText').value;
  const synonym = document.getElementById('dropdown').value;

  const addSynonymRequest = {
    NewWord: newWord,
    Synonym: synonym
  };

  try {
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
    console.log('New word added successfully.');
  } catch (error) {
    console.error('Network error:', error);
		return;
  }

	await populateDropdownWithWords();
}

async function fetchSynonyms(word) {
  const dto = {
    Value: word
  };

  try {
		console.log(`${apiUrl}${getSynonyms}`);
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
    const data = await response.json();
		console.log(data);
    return data || [];
  } catch (error) {
    console.error('Network error:', error);
    return [];
  }
}

function renderTable(synonymsResponse) {
	console.log("rendering table");	
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
	console.log(tableHtml);
}

async function handleFetchSynonyms() {
 const word = document.getElementById('inputText').value;
  fetchSynonyms(word)
    .then(synonyms => {			
      renderTable(synonyms);			
    });
}


populateDropdownWithWords();