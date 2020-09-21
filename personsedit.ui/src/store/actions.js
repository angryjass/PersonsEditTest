import axios from "axios";
axios.defaults.baseURL = "https://localhost:44323/api/";
let token = "";

export default {
  // eslint-disable-next-line no-unused-vars
  AuthPerson({ dispatch }, authProperties) {
    return new Promise((resolve, reject) => {
      axios
        .post(
          // eslint-disable-next-line no-undef
          "Account",
          {
            login: authProperties.login,
            password: authProperties.password
          }
        )
        .then(response => {
          token = response.data.access_token;
          resolve(response);
        })
        .catch(response => reject(alert(response)));
    });
  },
  GetPersons() {
    return new Promise((resolve, reject) => {
      axios
        // eslint-disable-next-line no-undef
        .get("Persons", {
          headers: { Authorization: "Bearer " + token }
        })
        .then(response => resolve(response.data))
        .catch(response => reject(alert(response)));
    });
  },
  // eslint-disable-next-line no-unused-vars
  DeletePerson({ dispatch }, personId) {
    return new Promise((resolve, reject) => {
      axios
        // eslint-disable-next-line no-undef
        .delete("Persons", {
          headers: {
            Authorization: "Bearer " + token
          },
          params: {
            personId: personId
          }
        })
        .then(response => resolve(response))
        .catch(response => reject(alert(response)));
    });
  },
  // eslint-disable-next-line no-unused-vars
  EditOrCreatePerson({ dispatch }, person) {
    return new Promise((resolve, reject) => {
      axios
        .post(
          // eslint-disable-next-line no-undef
          "Persons",
          {
            Person: person.person,
            Roles: Object.assign({}, person).roles
          },
          {
            headers: {
              Authorization: "Bearer " + token
            }
          }
        )
        .then(response => resolve(response))
        .catch(response => reject(alert(response)));
    });
  },
  GetRoles() {
    return new Promise((resolve, reject) => {
      axios
        // eslint-disable-next-line no-undef
        .get("Roles", {
          headers: { Authorization: "Bearer " + token }
        })
        .then(response => resolve(response.data))
        .catch(response => reject(alert(response)));
    });
  }
};
