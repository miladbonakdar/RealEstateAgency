<template>
  <div class="app flex-row align-items-center">
    <div class="container">
      <b-row class="justify-content-center">
        <b-col md="6" sm="8">
          <b-card no-body class="mx-4">
            <b-card-body class="p-4">
              <b-form>
                <h1>Register</h1>
                <p class="text-muted">Create your account</p>
                <b-input-group class="mb-3">
                  <b-input-group-prepend>
                    <b-input-group-text><i class="fa fa-industry"></i></b-input-group-text>
                  </b-input-group-prepend>
                  <b-form-input v-model="form.realEstateName" type="text" class="form-control" placeholder="Company name"/>
                </b-input-group>

                <b-input-group class="mb-3">
                  <b-input-group-prepend>
                    <b-input-group-text><i class="fa fa-user"></i></b-input-group-text>
                  </b-input-group-prepend>
                  <b-form-input  v-model="form.firstname" type="text" class="form-control" placeholder="Firstname"/>
                </b-input-group>

                <b-input-group class="mb-3">
                  <b-input-group-prepend>
                    <b-input-group-text><i class="fa fa-user"></i></b-input-group-text>
                  </b-input-group-prepend>
                  <b-form-input  v-model="form.lastname" type="text" class="form-control" placeholder="Lastname"/>
                </b-input-group>

                <b-input-group class="mb-3">
                  <b-input-group-prepend>
                    <b-input-group-text><i class="fa fa-user-circle-o"></i></b-input-group-text>
                  </b-input-group-prepend>
                  <b-form-input  v-model="form.username" type="text" class="form-control" placeholder="Username" autocomplete="username"/>
                </b-input-group>

                <b-input-group class="mb-3">
                  <b-input-group-prepend>
                    <b-input-group-text><i class="fa fa-at"></i></b-input-group-text>
                  </b-input-group-prepend>
                  <b-form-input  v-model="form.email" type="text" class="form-control" placeholder="Email" autocomplete="email"/>
                </b-input-group>

                <b-input-group class="mb-3">
                  <b-input-group-prepend>
                    <b-input-group-text><i class="fa fa-lock"></i></b-input-group-text>
                  </b-input-group-prepend>
                  <b-form-input  v-model="form.password" type="password" class="form-control" placeholder="Password"
                                autocomplete="new-password"/>
                </b-input-group>

                <b-input-group class="mb-4">
                  <b-input-group-prepend>
                    <b-input-group-text><i class="fa fa-lock"></i></b-input-group-text>
                  </b-input-group-prepend>
                  <b-form-input  v-model="form.passwordReapet" type="password" class="form-control" placeholder="Repeat password"
                                autocomplete="new-password"/>
                </b-input-group>

                <b-button variant="success" :disabled="registering" block @click="register">Create Account</b-button>
              </b-form>
            </b-card-body>
            <b-card-footer class="p-4">
              <b-row>
                <b-col cols="6">
                  <b-button block class="btn btn-facebook"><span>facebook</span></b-button>
                </b-col>
                <b-col cols="6">
                  <b-button block class="btn btn-google-plus" type="button"><span>Google</span></b-button>
                </b-col>
              </b-row>
            </b-card-footer>
          </b-card>
        </b-col>
      </b-row>
    </div>
  </div>
</template>

<script>
  export default {
    name: 'Register',
    data() {
      return {
        registering: false,
        form: {},
      }
    },
    methods: {
      register() {
        this.registering = true;
        this.$gate.auth.registerAgent(this.form).then(res => {
          localStorage.setItem("token", res.body.token);
          delete res.body.token;
          localStorage.setItem("user", JSON.stringify(res.body));
          localStorage.setItem("loggedOn", new Date().toString());
          this.$router.replaceLang('/dashboard');
        }).catch(err => console.log(err))
          .finally(() => this.registering = false)
      }
    }
  }
</script>
<style>

</style>

