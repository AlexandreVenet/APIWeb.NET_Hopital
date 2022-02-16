"use strict";

// __________________________________________________________________

// Fonctions générales

const uri = 'https://localhost:7032';

function UseURI(str)
{
    // Renvoyer une adresse complète à partir du point de terminaison fourni
	return `${uri}${str}`; 
}

// __________________________________________________________________

// Requête GET toutes les lignes

let contentDiv;

function CallGet(e)
{
	e.preventDefault();
	//console.log(e.target);

	fetch(UseURI('/Roles/Get all'))
		.then (response => response.json())
		.then (data => CallGetAllDisplayContent(data))
		.catch(error => console.error('Get ne fonctionne pas', error));
}

function CallGetAllDisplayContent(data)
{
	// console.log(data);
	contentDiv.innerHTML = '';

	let ol = document.createElement('ol');

	for (let i = 0; i < data.length; i++) {
		const element = data[i];
		// https://marian-caikovski.medium.com/innerhtml-vs-appendchild-e74c763846df
		const li = document.createElement('li');
		li.textContent = `Id : ${element.p_id}, role : ${element.p_role}`;
		ol.appendChild(li);
	}

	contentDiv.appendChild(ol);
}

// __________________________________________________________________

// Requête GET une ligne par id

let GetOneByIdForm;
let GetOneByIdContent;

function CallGetOne(e)
{
	e.preventDefault();

	const wantedIndex = GetOneByIdForm.elements['GetOneByIdFormId'].value;

	// https://javascript.info/xmlhttprequest
	// https://stackoverflow.com/questions/37690114/how-to-return-a-specific-status-code-and-no-contents-from-controller
	// https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#2xx_success
	const xhttp = new XMLHttpRequest();
	xhttp.open("GET", UseURI(`/Roles/Get role by ${wantedIndex}`), true);
	xhttp.onreadystatechange = function(e) {
		if (xhttp.readyState == 4)
		{
			console.log(xhttp.responseText);
			console.log(xhttp.statusText, xhttp.status);
			// console.log(JSON.parse(xhttp.responseText).messageRetour);
			
			if(xhttp.status == 200) 
			{
				const p = document.createElement('p');

				if(xhttp.responseText == "Rien")
				{
					p.textContent = 'Je n\'ai rien trouvé.';
				}
				else
				{
					const data = JSON.parse(xhttp.response);
					p.textContent = `Id : ${data.p_id}, role : ${data.p_role}`; // nom des propriétés du model
				}
				GetOneByIdContent.innerHTML = '';
				GetOneByIdContent.appendChild(p);
			}

		}
	};
	xhttp.send(); 
}

// __________________________________________________________________

// Requête GET l'id par name

let GetOneByNameForm;
let GetOneByNameContent;

function CallGetIdByName(e)
{
	e.preventDefault();
	
	const name = GetOneByNameForm.elements['Name'].value;
	
	if(name == null  || name == '') 
	{
		const p = document.createElement('p');
		p.textContent = 'Formulaire invalide';
		GetOneByNameContent.innerHTML = '';
		GetOneByNameContent.appendChild(p);
		return;
	}

	const xhttp = new XMLHttpRequest();
	xhttp.open("GET", UseURI(`/Roles/Get id by ${name}`), true);
	xhttp.onreadystatechange = function(e) {
		if (xhttp.readyState == 4)
		{		
			console.log(xhttp.responseText);
			console.log(xhttp.statusText, xhttp.status);
			// console.log(JSON.parse(xhttp.responseText).messageRetour);

			if(xhttp.status == 200) 
			{
				const p = document.createElement('p');

				if(xhttp.responseText == "Rien")
				{
					p.textContent = 'Je n\'ai rien trouvé.';
				}
				else
				{
					const data = JSON.parse(xhttp.response);
					p.textContent = `Id : ${data}`;
				}
				GetOneByNameContent.innerHTML = '';
				GetOneByNameContent.appendChild(p);
			}
		}
	};
	xhttp.send(); 
}

// __________________________________________________________________

// Requête POST par name

let PostByNameForm;
let PostByNameContent;

function CallPostByName(e)
{
	e.preventDefault();

	const name = PostByNameForm.elements['Name'].value;
	
	if(name == null  || name == '') 
	{
		const p = document.createElement('p');
		p.textContent = 'Formulaire invalide';
		PostByNameContent.innerHTML = '';
		PostByNameContent.appendChild(p);
		return;
	}

	const form = document.forms.PostByNameForm;
	const formData = new FormData(form);
	const itemsToAdd = 
	{
		name: formData.get('Name'),
	}

	// console.log(itemsToAdd);

	const xhr = new XMLHttpRequest();
	xhr.open('POST', UseURI(`/Roles/Post by ${name}`), true);
	xhr.onreadystatechange = function(e) {
		if (xhr.readyState == 4)
		{		
			// console.log(xhr.responseText);
			console.log(xhr.statusText, xhr.status);
			// console.log(JSON.parse(xhttp.responseText).messageRetour);

			const p = document.createElement('p');
			
			if(xhr.status == 201) 
			{
				p.textContent = 'Insertion ok';

			}
			else if(xhr.status == 200)
			{
				if(xhr.responseText == "Déjà")
				{
					p.textContent = 'Cette entrée existe déjà';
				}
			}

			PostByNameContent.innerHTML = '';
			PostByNameContent.appendChild(p);
		}
	};
	xhr.send(itemsToAdd); 
}

// __________________________________________________________________

// Requête PUT par id et name

let UpdateByIdRoleForm;
let UpdateByIdRoleContent;

function CallUpdateByIdRole(e)
{
	e.preventDefault();

	const id = UpdateByIdRoleForm.elements['Id'].value;
	const role = UpdateByIdRoleForm.elements['Role'].value;

	if(id == null || role == null  || id == '' || role == '') 
	{
		const p = document.createElement('p');
		p.textContent = 'Formulaire invalide';
		UpdateByIdRoleContent.innerHTML = '';
		UpdateByIdRoleContent.appendChild(p);
		return;
	}
	
	const form = document.forms.UpdateByIdRoleForm;
	const formData = new FormData(form);
	const itemsToAdd = 
	{
		id: formData.get('Id'),
		role: formData.get('Role'),
	}
	// console.log(itemsToAdd);

	const xhr = new XMLHttpRequest();
	xhr.open('PUT', UseURI(`/Roles/Update by ${id} ${role}`), true);
	xhr.onreadystatechange = function(e) {
		if (xhr.readyState == 4)
		{		
			console.log(xhr.statusText, xhr.status, xhr.responseText);
			// console.log(JSON.parse(xhttp.responseText).messageRetour);

			const p = document.createElement('p');
			
			if(xhr.status == 204) 
			{
				p.textContent = 'Modification ok';

			}
			else if(xhr.status == 200)
			{
				if(xhr.responseText == "Rien")
				{
					p.textContent = 'Cette entrée n\'existe pas';
				}
				else if(xhr.responseText == 'Oula')
				{
					p.textContent = 'Requête avec des champs qui puent.';
				}
			}

			UpdateByIdRoleContent.innerHTML = '';
			UpdateByIdRoleContent.appendChild(p);
		}
	};
	xhr.send(itemsToAdd); 
}

// __________________________________________________________________

// Requête PUT par id et name

let DeleteByIdForm;
let DeleteByIdContent;

function CallDeleteById(e)
{
	e.preventDefault();

	const id = DeleteByIdForm.elements['Id'].value;

	if(id == null || id == '') 
	{
		const p = document.createElement('p');
		p.textContent = 'Formulaire invalide';
		DeleteByIdContent.innerHTML = '';
		DeleteByIdContent.appendChild(p);
		return;
	}

	const form = document.forms.DeleteByIdForm;
	const formData = new FormData(form);
	const itemsToAdd = 
	{
		id: formData.get('Id'),
	}
	// console.log(itemsToAdd);

	const xhr = new XMLHttpRequest();
	xhr.open('DELETE', UseURI(`/Roles/Delete by ${id}`), true);
	xhr.onreadystatechange = function(e) {
		if (xhr.readyState == 4)
		{		
			console.log(xhr.statusText, xhr.status, xhr.responseText);
			// console.log(JSON.parse(xhttp.responseText).messageRetour);

			const p = document.createElement('p');
			
			if(xhr.status == 200)
			{
				if(xhr.responseText == 'Oula')
				{
					p.textContent = 'Requête avec des champs qui puent.';
				}
				else if(xhr.responseText == 'Rien')
				{
					p.textContent = 'Cette entrée n\'existe pas.';
				}
			}
			else if(xhr.status == 204)
			{
				p.textContent = 'Suppression ok';
			}

			DeleteByIdContent.innerHTML = '';
			DeleteByIdContent.appendChild(p);
		}
	};
	xhr.send(itemsToAdd); 
}

// __________________________________________________________________

// DOSSIERS - Requête PUT par id et options

let DossiersUpdateForm;
let DossiersUpdateContent;

function DossiersCallUpdate(e)
{
	e.preventDefault();

	// construire le paquet de données à envoyer et s'en servir pour en tester le contenu

	const form = document.forms.DossiersUpdateForm;
	const formData = new FormData(form);
	const itemsToAdd = 
	{
		id: formData.get('id'),
	}

	if(itemsToAdd.id == null || itemsToAdd.id == '') 
	{
		const p = document.createElement('p');
		p.textContent = 'Formulaire invalide';
		DossiersUpdateContent.innerHTML = '';
		DossiersUpdateContent.appendChild(p);
		return;
	}

	if(formData.get('dateEntree') != '') itemsToAdd.dateEntree = formData.get('dateEntree');
	if(formData.get('dateSortie') != '') itemsToAdd.dateSortie = formData.get('dateSortie');
	if(formData.get('motif') != '') itemsToAdd.motif = formData.get('motif');
	console.log(itemsToAdd);

	// construire la requête
	let rq = `/Dossiers/Update by ${itemsToAdd.id}, one or more medical values`;
	if(itemsToAdd.dateEntree || itemsToAdd.dateSortie || itemsToAdd.motif) rq += '?';
	if(itemsToAdd.dateEntree) rq += `dateEntree=${itemsToAdd.dateEntree}`; 
	if(itemsToAdd.dateSortie || itemsToAdd.motif) rq += '&';
	if(itemsToAdd.dateSortie) rq += `dateSortie=${itemsToAdd.dateSortie}`;
	if(itemsToAdd.motif) rq += '&';
	if(itemsToAdd.motif) rq += `motif=${itemsToAdd.motif}`;

	const xhr = new XMLHttpRequest();
	xhr.open('PUT', UseURI(rq), true);
	xhr.onreadystatechange = function(e) {
		if (xhr.readyState == 4)
		{		
			console.log(xhr.statusText, xhr.status, xhr.responseText);
			// console.log(JSON.parse(xhttp.responseText).messageRetour);

			const p = document.createElement('p');
			
			if(xhr.status == 200)
			{
				if(xhr.responseText == 'Oula')
				{
					p.textContent = 'Ce formulaire est incomplet.';
				}
				else if(xhr.responseText == 'Ouladate')
				{
					p.textContent = 'Elles puent, tes dates.';
				}
				else if(xhr.responseText == 'Rien')
				{
					p.textContent = 'Ce dossier n\'existe pas.';
				}
				else if(xhr.responseText == 'True')
				{
					p.textContent = 'Modification ok';
				}
			}
			// else if(xhr.status == 204)
			// {
			// 	p.textContent = 'Suppression ok';
			// }

			DossiersUpdateContent.innerHTML = '';
			DossiersUpdateContent.appendChild(p);
		}
	};
	xhr.send(itemsToAdd); 
}

// __________________________________________________________________

// Démarrage 

window.addEventListener('DOMContentLoaded', () => {
	console.log('DOM prêt mais image, cadres et autres ressources en chargement.');
});

window.addEventListener('load', () => {
	console.log('DOM et ressources Chargés complètement.');

	// Get All

	document.getElementById('getAllButton').addEventListener('click', CallGet, false);
	contentDiv = document.getElementById('getAllContent');

	// Get By Id

	GetOneByIdForm = document.getElementById('GetOneByIdForm');
	GetOneByIdForm.addEventListener('submit', CallGetOne, false);
    GetOneByIdContent = document.getElementById('GetOneByIdContent');

	// Get By Name

	GetOneByNameForm = document.getElementById('GetOneByNameForm');
	GetOneByNameForm.addEventListener('submit', CallGetIdByName, false);
    GetOneByNameContent = document.getElementById('GetOneByNameContent');

	// Post by name

	PostByNameForm = document.getElementById('PostByNameForm');
	PostByNameForm.addEventListener('submit', CallPostByName, false);
	PostByNameContent = document.getElementById('PostByNameContent');

	// Put by id and role

	UpdateByIdRoleForm = document.getElementById('UpdateByIdRoleForm');
	UpdateByIdRoleForm.addEventListener('submit', CallUpdateByIdRole, false);
	UpdateByIdRoleContent = document.getElementById('UpdateByIdRoleContent');

	// Delete by id

	DeleteByIdForm = document.getElementById('DeleteByIdForm');
	DeleteByIdForm.addEventListener('submit',CallDeleteById,false);
	DeleteByIdContent = document.getElementById('DeleteByIdContent');

	// DOSSIERS - Put by id et options

	DossiersUpdateForm = document.getElementById('DossiersUpdateForm');
	DossiersUpdateForm.addEventListener('submit',DossiersCallUpdate,false);
	DossiersUpdateContent = document.getElementById('DossiersUpdateContent');

});

// __________________________________________________________________
