<template>
  <v-app id="app">
    <v-app-bar app color="indigo" dark>
      <v-toolbar-title>Добро пожаловать, {{ Username }}!</v-toolbar-title>
      <v-spacer></v-spacer>
      <v-dialog v-model="showDialog" max-width="500px">
        <template v-slot:activator="{ on, attrs }">
          <v-btn class="ma-10" @click="ShowCreateModal" v-bind="attrs" v-on="on">
            <v-icon>mdi-plus</v-icon>1 пользователь
          </v-btn>
        </template>
        <v-form v-model="validPerson">
          <v-card>
            <v-card-text>
              <v-container>
                <v-text-field
                  label="Имя пользователя"
                  v-model="editedPerson.person.name"
                  :counter="25"
                  :rules="nameRules"
                  required
                />
                <v-text-field
                  label="Email"
                  :rules="emailRules"
                  v-model="editedPerson.person.email"
                />
                <v-text-field
                  label="Логин"
                  v-model="editedPerson.person.login"
                  :disabled="!isCreateModal"
                  :rules="loginRules"
                  required
                />
                <v-text-field
                  label="Пароль"
                  v-model="editedPerson.person.password"
                  :rules="passwordRules"
                  required
                />
                <v-select
                  multiple
                  label="Роли"
                  v-model="editedPerson.roles"
                  :items="roles"
                  item-text="name"
                  return-object
                />
              </v-container>
            </v-card-text>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="blue darken-1" text @click="CloseModal">Cancel</v-btn>
              <v-btn
                color="blue darken-1"
                text
                @click="EditOrCreatePerson(editedPerson)"
                :disabled="!validPerson"
              >Save</v-btn>
            </v-card-actions>
          </v-card>
        </v-form>
      </v-dialog>
      <v-dialog v-model="authDialog" fullscreen hide-overlay transition>
        <template v-slot:activator="{ on, attrs }">
          <v-icon @click="ExitAccount" v-bind="attrs" v-on="on" large>mdi-exit-to-app</v-icon>
        </template>
        <v-card>
          <v-container class="fill-height" fluid>
            <v-row align="center" justify="center">
              <v-col cols="12" sm="8" md="4">
                <v-card class="elevation-12">
                  <v-toolbar color="primary" dark flat>
                    <v-toolbar-title>Авторизация</v-toolbar-title>
                  </v-toolbar>
                  <v-card-text>
                    <v-form>
                      <v-text-field
                        label="Логин"
                        prepend-icon="mdi-account"
                        type="text"
                        v-model="authProperties.login"
                      ></v-text-field>

                      <v-text-field
                        label="Пароль"
                        prepend-icon="mdi-lock"
                        type="password"
                        v-model="authProperties.password"
                      ></v-text-field>
                    </v-form>
                  </v-card-text>
                  <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn @click="AuthPerson" color="primary">Вход</v-btn>
                  </v-card-actions>
                </v-card>
              </v-col>
            </v-row>
          </v-container>
        </v-card>
      </v-dialog>
    </v-app-bar>
    <v-main>
      <v-container fluid>
        <v-simple-table height="500px" :fixed-header="Boolean(true)" v-if="persons.length > 0">
          <thead>
            <tr>
              <th>Имя</th>
              <th>Логин</th>
              <th>Роли</th>
              <th>Действия</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="person in persons" :key="person.id">
              <td>{{person.person.name}}</td>
              <td>{{person.person.login}}</td>
              <td>{{person.roles.map(p => p.name).toString()}}</td>
              <td>
                <v-icon class="mr-2" small @click="ShowEditModal(person)">mdi-pencil</v-icon>
                <v-icon class="mr-2" small @click="DeletePerson(person)">mdi-delete</v-icon>
              </td>
            </tr>
          </tbody>
        </v-simple-table>
      </v-container>
    </v-main>
  </v-app>
</template>

<script>
export default {
  name: "App",
  data() {
    return {
      isCreateModal: true,
      editPersonIndex: -1,
      editedPerson: {
        person: {},
        roles: [],
      },
      authProperties: {
        login: "",
        pasword: "",
      },
      roles: [],
      authDialog: true,
      showDialog: false,
      persons: [],
      userName: "",

      emailRules: [
        (v) => !!v || "",
        (v) => /.+@.+\..+/.test(v) || "Неверный email адрес",
      ],
      nameRules: [
        (v) => !!v || "Имя обязательно",
        (v) => (v && v.length <= 25) || "Имя должно иметь до 25 символов",
      ],
      loginRules: [(v) => !!v || "Login обязателен"],
      passwordRules: [(v) => !!v || "Password обязателен"],
      validPerson: false,
    };
  },
  computed: {
    Username() {
      return this.userName;
    },
  },
  methods: {
    GetPersons() {
      this.$store
        .dispatch("GetPersons")
        .then((response) => (this.persons = response));
    },

    EditOrCreatePerson(person) {
      this.$store.dispatch("EditOrCreatePerson", person).then((response) => {
        this.showDialog = false;
        this.editedPerson = {
          person: {},
          roles: [],
        };
        this.editPersonIndex = -1;
        if (response.data != "") {
          this.persons.push(response.data);
        } else {
          this.persons.splice(this.editPersonIndex, 1, person);
        }
      });
    },

    DeletePerson(person) {
      let personIndex = this.persons.indexOf(person);
      confirm("Вы уверены, что хотите удалить эту запись?") &&
        this.$store
          .dispatch("DeletePerson", person.person.id)
          .then(() => this.persons.splice(personIndex, 1));
    },

    CloseModal() {
      this.editedPerson = {
        person: {},
        roles: [],
      };
      this.showDialog = false;
    },

    ShowEditModal(person) {
      this.isCreateModal = false;
      this.$store
        .dispatch("GetRoles")
        .then((response) => (this.roles = response));

      this.editPersonIndex = this.persons.indexOf(person);
      this.editedPerson = Object.assign({}, JSON.parse(JSON.stringify(person)));
      this.showDialog = true;
    },

    ShowCreateModal() {
      this.isCreateModal = true;
      this.$store
        .dispatch("GetRoles")
        .then((response) => (this.roles = response));
    },

    ExitAccount() {
      this.authDialog = true;
      this.persons = [];
    },

    AuthPerson() {
      this.$store
        .dispatch("AuthPerson", this.authProperties)
        .then((response) => {
          if (response.status === 200) {
            this.userName = response.data.name;
            this.authDialog = false;
            this.GetPersons();
          }
        });
    },
  },
};
</script>

<style>
</style>
