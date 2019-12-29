<template>
  <b-card header="New agent">
    <b-row>
      <div class="col-md-4">
        <b-form-group label="First name">
          <b-form-input
            @focus="clear('firstName')"
            type="text"
            v-model="agentForm.firstName"
            placeholder="Enter first name"
          ></b-form-input>
          <div
            v-if="errmsg.firstName && hasError"
            class="error vuelidate-invalid-feedback"
          >First name is required.
          </div>
        </b-form-group>
      </div>
      <div class="col-md-4">
        <b-form-group label="Last name">
          <b-form-input
            @focus="clear('lastName')"
            type="text"
            v-model="agentForm.lastName"
            placeholder="Enter last name"
          ></b-form-input>
          <div
            v-if="errmsg.lastName && hasError"
            class="error vuelidate-invalid-feedback"
          >Last name is required.
          </div>
        </b-form-group>
      </div>
      <div class="col-md-4">
        <b-form-group label="Middle name">
          <b-form-input type="text" v-model="agentForm.middleName" placeholder="Enter middle name"></b-form-input>
        </b-form-group>
      </div>
      <div class="col-md-4">
        <b-form-group label="Username">
          <b-form-input
            :disabled="isEditMode"
            type="text"
            @focus="clear('userName')"
            v-model="agentForm.userName"
            placeholder="Enter username"
          ></b-form-input>
          <div
            v-if="errmsg.userName && hasError"
            class="error vuelidate-invalid-feedback"
          >Username is required.
          </div>
        </b-form-group>
      </div>

      <div class="col-md-4">
        <b-form-group label="Passwrod">
          <b-form-input
            :disabled="isEditMode"
            type="password"
            v-model="agentForm.password"
            @focus="clear('password')"
            placeholder="Enter password"
          ></b-form-input>
          <div
            v-if="errmsg.password && hasError"
            class="error vuelidate-invalid-feedback"
          >Password is required.
          </div>
        </b-form-group>
      </div>
      <div class="col-md-4">
        <b-form-group label="Email">
          <b-form-input
            :disabled="isEditMode"
            type="email"
            v-model="agentForm.email"
            @focus="clear('email')"
            placeholder="Enter email"
          ></b-form-input>
          <div
            v-if="errmsg.email && hasError"
            class="error vuelidate-invalid-feedback"
          >Email is not valid!
          </div>
        </b-form-group>
      </div>
      <div class="col-md-4">
        <b-form-group label="Phone1">
          <b-form-input type="text" v-model="agentForm.phone01" placeholder="Enter phone1"></b-form-input>
        </b-form-group>
      </div>
      <div class="col-md-4">
        <b-form-group label="Phone2">
          <b-form-input type="text" v-model="agentForm.phone02" placeholder="Enter phone2"></b-form-input>
        </b-form-group>
      </div>
      <div class="col-md-4">
        <b-form-group label="Address01">
          <b-form-input type="text" v-model="agentForm.address01" placeholder="Enter address01"></b-form-input>
        </b-form-group>
      </div>
      <div class="col-md-4">
        <b-form-group label="Address02">
          <b-form-input type="text" v-model="agentForm.address02" placeholder="Enter address02"></b-form-input>
        </b-form-group>
      </div>
      <div class="col-md-4">
        <b-form-group label="Zip code">
          <b-form-input type="text" v-model="agentForm.zipCode" placeholder="Enter zip code"></b-form-input>
        </b-form-group>
      </div>
      <div class="col-md-4 py-4">
        <b-form-group>
          <b-form-checkbox v-model="agentForm.isResponsible">Is responsible?</b-form-checkbox>
        </b-form-group>
        <b-form-group>
          <b-form-checkbox
            v-model="agentForm.hasPublishingAuthorization"
          >Has publishing authorization?
          </b-form-checkbox>
        </b-form-group>
      </div>
      <div class="col-md-12">
        <b-form-textarea
          id="textarea"
          v-model="agentForm.description"
          placeholder="Enter something..."
          rows="3"
          max-rows="6"
        ></b-form-textarea>
      </div>
    </b-row>
    <submit-group v-on:onCancel="onCancel" v-on:onSubmit="checkUpdate"/>
  </b-card>
</template>
<script>
    import {mapMutations, mapActions} from "vuex";
    import {statics} from "../../store/types";

    export default {
        data() {
            return {
                agentForm: {
                    firstName: "",
                    lastName: "",
                    middleName: "",
                    userName: "",
                    password: "",
                    email: "",
                    phone01: "",
                    phone02: "",
                    address01: "",
                    address02: "",
                    zipCode: "",
                    description: "",
                    isResponsible: false,
                    hasPublishingAuthorization: false
                },
                errmsg: {
                    userName: false,
                    firstName: false,
                    lastName: false,
                    password: false,
                    email: false
                },
                hasError: false
            };
        },
        created() {
            if (this.isEditMode) this.setProperty();
        },
        methods: {
            ...mapMutations({
                showLoading: statics.mutations.loading
            }),
            ...mapActions({
                loadAgents: statics.actions.loadAgents
            }),
            checkValidations() {
                if (this.agentForm.firstName == "") {
                    this.errmsg.firstName = true;
                    this.hasError = true;
                }
                if (this.agentForm.lastName == "") {
                    this.errmsg.lastName = true;
                    this.hasError = true;
                }
                if (this.agentForm.userName == "") {
                    this.errmsg.userName = true;
                    this.hasError = true;
                }
                if (this.agentForm.password == "") {
                    this.errmsg.password = true;
                    this.hasError = true;
                }
                if (!(/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(this.agentForm.email))) {
                    this.errmsg.email = true;
                    this.hasError = true;
                }
            },
            clear(item) {
                switch (item) {
                    case "firstName":
                        this.hasError = false;
                        this.errmsg.firstName = false;
                        break;
                    case "lastName":
                        this.hasError = false;
                        this.errmsg.lastName = false;
                        break;
                    case "userName":
                        this.hasError = false;
                        this.errmsg.userName = false;
                        break;
                    case "password":
                        this.hasError = false;
                        this.errmsg.password = false;
                        break;
                    case "email":
                        this.hasError = false;
                        this.errmsg.email = false;
                        break;
                }
            },
            setProperty() {
                this.showLoading(true);
                this.$gate.agent
                    .getAgentForUpdate(this.$route.params.id)
                    .then(res => {
                        this.agentForm = res.body;
                    debugger;
                    })
                    .catch(err => this.$handleError(err))
                    .finally(() => this.showLoading(false));
            },

            createAgent() {
                this.checkValidations();
                if (!this.hasError) {
                    this.showLoading(true);
                    this.$gate.agent
                        .createAgent(this.agentForm)
                        .then(res => {
                            this.loadAgents(this.$gate);
                            this.$toasted.success("Agent created successfully.");
                            this.$router.pushLang(`agents/list`);
                        })
                        .catch(err => this.$handleError(err))
                        .finally(() => {
                            this.showLoading(false);
                        });
                }
            },
            updateAgent() {
                this.checkValidations();
                if (!this.hasError) {
                    this.showLoading(true);
                    this.agentForm["updatePassword"] = false;
                    this.$gate.agent
                        .updateAgent(this.agentForm)
                        .then(res => {
                            this.$toasted.success("Agent details updated.");
                        })
                        .catch(err => this.$handleError(err))
                        .finally(() => this.showLoading(false));
                }
            },
            onCancel() {
                this.$router.pushLang("/agents/list");
            },
            checkUpdate() {
                this.isEditMode ? this.updateAgent() : this.createAgent();
            }
        },
        computed: {
            isEditMode() {
                return this.$route.params.id !== "new";
            }
        }
    };
</script>
<style scoped>
  .vuelidate-invalid-feedback {
    width: 100%;
    margin-top: 0.25rem;
    font-size: 80%;
    color: #f86c6b;
  }
</style>
