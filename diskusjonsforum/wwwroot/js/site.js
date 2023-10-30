
// Get the button:
let mybutton = document.getElementById("myBtn");

// When the user scrolls down 20px from the top of the document, show the button
window.onscroll = function () { scrollFunction() };

function scrollFunction() {
    if (document.body.scrollTop > 5 || document.documentElement.scrollTop > 5) {
        mybutton.style.display = "block";
    } else {
        mybutton.style.display = "none";
    }
}

// When the user clicks on the button, scroll to the top of the document
function topFunction() {
    document.body.scrollTop = 0; // For Safari
    document.documentElement.scrollTop = 0; // For Chrome, Firefox, IE and Opera
}

function handleSearchInput() {
    const searchInput = document.getElementById('searchInput');
    const searchResults = document.getElementById('searchResultsDropdown'); // Updated to use the new id

    let searchTerm = searchInput.value.toLowerCase();

    if (searchTerm.length >= 2) {
        // Use the correct URL for your controller
        fetch(`/Thread/SearchPost?searchQuery=${searchTerm}`)
            .then(response => response.json())
            .then(data => {
                // Handle the JSON response
                console.log(data);

                // Clear previous search results
                searchResults.innerHTML = '';

                if (data.length > 0) {
                    // Create a dropdown or list to display the search results
                    const resultList = document.createElement('ul');
                    resultList.classList.add('search-results');

                    // Loop through the search results and create list items
                    data.forEach(result => {
                        const listItem = document.createElement('li');
                        listItem.textContent = result.threadTitle;
                        // Add a click event listener to the list item to handle selection
                        listItem.addEventListener('click', () => {
                            // Handle the selection, e.g., navigate to the selected thread
                            // You can use the result.threadId to identify the selected thread
                        });

                        resultList.appendChild(listItem);
                    });

                    // Append the result list to the searchResults container
                    searchResults.appendChild(resultList);

                    // Display the results container
                    searchResults.style.display = 'block';
                } else {
                    searchResults.style.display = 'none'; // No results, hide the container
                }
            });
    } else {
        searchResults.style.display = 'none';
    }
}


