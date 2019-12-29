export default (dataPromise,done) => {
  dataPromise.then(res => {
    if (!res.ok) throw new Error(res);
    done(res.data);
  }).catch(err => {
    console.log(err);
    throw err;
  })
}
