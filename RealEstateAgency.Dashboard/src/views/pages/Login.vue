<template>
  <div class="login flex-row align-items-center">
    <div class="container">
      <b-row class="justify-content-center">
        <b-col md="8">
          <b-card-group>
            <b-card no-body class="p-4">
              <b-card-body>
                <b-form>
                  <h1>Login</h1>
                  <p class="text-muted">Sign In to your account</p>
                  <b-input-group class="mb-3">
                    <b-input-group-prepend>
                      <b-input-group-text>
                        <i class="icon-user"></i>
                      </b-input-group-text>
                    </b-input-group-prepend>
                    <b-form-input
                      v-model="form.usernameOrEmail"
                      type="text"
                      class="form-control"
                      name="username"
                      placeholder="Username or email"
                      autocomplete="username email"
                      v-on:keyup="eneterKey($event)"
                      @focus="clear('username')"
                    />
                    <div
                      v-if="errmsg.username && hasError"
                      class="error vuelidate-invalid-feedback"
                    >Username or email is required.
                    </div>
                  </b-input-group>

                  <b-input-group class="mb-4">
                    <b-input-group-prepend>
                      <b-input-group-text>
                        <i class="icon-lock"></i>
                      </b-input-group-text>
                    </b-input-group-prepend>
                    <b-form-input
                      v-model="form.password"
                      type="password"
                      name="password"
                      class="form-control"
                      v-on:keyup="eneterKey($event)"
                      placeholder="Password"
                      autocomplete="current-password"
                      @focus="clear('password')"
                    />
                    <div
                      v-if="errmsg.password && hasError"
                      class="error vuelidate-invalid-feedback"
                    >Password is required.
                    </div>
                  </b-input-group>

                  <b-row>
                    <b-col cols="6">
                      <b-button variant="primary" :disabled="signingIn" class="px-4 py-2" @click="signIn"><i
                        v-if="signingIn" class="fa fa-circle-o-notch fa-spin"></i> Login
                      </b-button>
                    </b-col>
                    <b-col cols="6" class="text-right">
                      <b-button variant="link" class="px-0">Forgot password?</b-button>
                    </b-col>
                  </b-row>
                </b-form>
              </b-card-body>
            </b-card>
          </b-card-group>
        </b-col>
      </b-row>
    </div>
  </div>
</template>

<script>
    export default {
        name: "Login",
        data() {
            return {
                form: {},
                signingIn: false,
                errmsg: {
                    username: false,
                    password: false
                },
                hasError: false
            };
        },
        created() {
            if(localStorage.getItem("token"))
                this.$router.replaceLang("/dashboard");
        },
        methods: {
            eneterKey(e) {
                if (e.keyCode === 13) {
                    this.validateUser();
                    this.hasError == false ? this.signIn() : '';
                }
            },
            validateUser() {
                if (this.form.usernameOrEmail == "") {
                    this.hasError = true;
                    this.errmsg.username = true;
                }
                if (this.form.password == "") {
                    this.hasError = true;
                    this.errmsg.password = true;
                }
            },
            gotoRegister() {
                this.$router.replaceLang("/auth/register");
            },
            signIn() {
                this.validateUser();
                if (this.hasError == false) {
                    this.signingIn = true;
                    this.$gate.auth
                        .login(this.form)
                        .then(res => {
                            localStorage.setItem("token", res.body.token);
                            delete res.body.token;
                            localStorage.setItem("user", JSON.stringify(res.body));
                            localStorage.setItem("loggedOn", new Date().toString());
                            this.$router.replaceLang("/dashboard");
                        })
                        .catch(err => this.$toasted.global.error(err.body.Message))
                        .finally(() => (this.signingIn = false));
                }
            },
            clear(item) {
                if (item == "username") {
                    this.errmsg.username = false;
                    this.hasError = false;
                }
                if (item == "password") {
                    this.errmsg.password = false;
                    this.hasError = false;
                }
            }
        }
    };
</script>
<style scoped>
  .login {
    margin-top: 100px;
  }

  .vuelidate-invalid-feedback {
    width: 100%;
    margin-top: 0.25rem;
    font-size: 80%;
    color: #f86c6b;
  }
</style>
